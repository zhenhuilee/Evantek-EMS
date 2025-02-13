<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <q-page>
        <div class="table-container">
          <q-table
            class="full-width styled-table"
            flat
            bordered
            dense
            :hide-pagination="true"
            :rows="table1Rows"
            :columns="columns"
            row-key="name"
            :rows-per-page-options="[0]"
          >
            <template v-slot:body-cell-note="props">
              <q-td :props="props">
                <q-scroll-area
                  :visible="false"
                  style="height: 100%; max-width: 100%"
                  :delay="0"
                  :ref="
                    (el: any) =>
                      scrollAreaRefs.indexOf(el) < 0
                        ? scrollAreaRefs.push(el)
                        : null
                  "
                >
                  <div class="row no-wrap">
                    {{ props.value }}
                  </div>
                </q-scroll-area>
              </q-td>
            </template>
          </q-table>

          <q-table
            class="full-width styled-table"
            flat
            bordered
            dense
            :hide-pagination="true"
            :rows="table2Rows"
            :columns="columns"
            row-key="name"
            :rows-per-page-options="[0]"
          >
            <template v-slot:body-cell-note="props">
              <q-td :props="props">
                <q-scroll-area
                  :visible="false"
                  style="height: 100%; max-width: 100%"
                  :delay="0"
                  :ref="
                    (el: any) =>
                      scrollAreaRefs.indexOf(el) < 0
                        ? scrollAreaRefs.push(el)
                        : null
                  "
                >
                  <div class="row no-wrap">
                    {{ props.value }}
                  </div>
                </q-scroll-area>
              </q-td>
            </template>
          </q-table>
        </div>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref, onMounted} from 'vue';
import { useQuasar, QTableColumn,date } from 'quasar';
import { appApi } from 'boot/axios'; // Import the configured axios instance
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';


const $q = useQuasar();
let connection: HubConnection = null;
let lock = false;
let triggerUpdateIntervalId: NodeJS.Timeout;

const scrollAreaRefs = ref([]);

const columns: QTableColumn[] = [
  {
    name: 'name',
    label: 'Name',
    field: 'userName',
    align: 'left',
    style: 'width: 20%',
  },
  {
    name: 'status',
    label: 'Status',
    field: 'statusName',
    align: 'left',
    style: 'width: 20%',
  },
  {
    name: 'lastupdated',
    label: ' Updated',
    field: 'lastUpdated',
    format: (val) => convertDateTime(val),
    align: 'left',
    style: 'width: 20%',
  },
  {
    name: 'note',
    label: 'Note',
    field: 'note',
    align: 'left',
    style: 'width: 40%',
  },  
];

const table1Rows = ref([]);
const table2Rows = ref([]);

async function fetchData() {
  try {
    const response = await appApi.get('/api/Display/GetDisplayList'); // Use appApi instance for the API call

    const displayList = response.data;

    // Sort the display list by userName in alphabetical order
    displayList.sort((a: { userName: string }, b: { userName: any }) =>
      a.userName.localeCompare(b.userName)   
    );

    // Split the display list into two tables
    const halfIndex = Math.ceil(displayList.length / 2);
    table1Rows.value = displayList.slice(0, halfIndex);
    table2Rows.value = displayList.slice(halfIndex);

    console.log(table1Rows);
    console.log(table2Rows);

    setupUpdateTimer();
  } catch (error) {
    console.error('Error fetching data:', error);
    $q.notify({ color: 'negative', message: 'Failed to fetch data' });
  }
}


function convertDateTime(dateTime: Date) {
  if (!dateTime) {
    return 'N/A';
    return dateTime?.toString() ?? 'NUL';
  }

  const currentDate = Date.now();
  const formatCurrentDate = date.formatDate(currentDate, 'DD/MM');

  const updatedDate = new Date(dateTime);
  const formatUpdatedDate = date.formatDate(updatedDate, 'DD/MM');

 if (formatCurrentDate == formatUpdatedDate){
  return date.formatDate(updatedDate, 'HH:mm');
 } else {
  return date.formatDate(updatedDate, 'DD/MM HH:mm');
 }
}


onMounted(() => {
  setupSignalR();
  fetchData();
  setTimeout(scrollForwards, 3000);
});

function setupUpdateTimer() {
  //Make sure this is called only once
  if (triggerUpdateIntervalId != null) {
    return;
  }

  triggerUpdateIntervalId = setInterval(() => {
    if (!lock) {
      //!!! This function doesn't do anything except
      //to trigger the data to 'changed' in order for last updated time
      //to update every second
      let temp1List = table1Rows.value;
      let temp2List = table2Rows.value;

      temp1List.forEach((element) => {
        var temp = element.lastUpdated;
        element.lastUpdated = null;
        element.lastUpdated = temp;
      });

      temp2List.forEach((element) => {
        var temp = element.lastUpdated;
        element.lastUpdated = null;
        element.lastUpdated = temp;
      });
    }
  }, 1000);
}

function scrollForwards() {
  scrollAreaRefs.value.forEach((el) => {
    el.setScrollPercentage('horizontal', 1.0, 3000);
  });

  setTimeout(jumpToScrollStart, 6000);
}

function jumpToScrollStart() {
  scrollAreaRefs.value.forEach((el) => {
    el.setScrollPosition('horizontal', 0.0);
  });

  setTimeout(scrollForwards, 2000);
}

async function setupSignalR() {
  var api = appApi.getUri() + '/displayHub';
  connection = new HubConnectionBuilder()
    .withUrl(api, {
      accessTokenFactory: () => localStorage.getItem('token'),
    })
    .build();

  connection.on('SendMessage', function (user, message) {
    console.log(user + ': ' + message);
  });

  connection.on(
    'UpdateUserStatus',
    function (userId, statusName, note, lastUpdated) {
      console.log(userId + ': ' + statusName + note);
      lock = true;

      var user = table1Rows.value.find((x) => x.userId == userId);
      if (user != null) {
        user.note = note;
        user.statusName = statusName;
        user.lastUpdated = lastUpdated;
      }

      user = table2Rows.value.find((x) => x.userId == userId);
      if (user != null) {
        user.note = note;
        user.statusName = statusName;
        user.lastUpdated = lastUpdated;
      }
      lock = false;
    }
  );

  
  // add user
  connection.on(
    'AddUser',
    function (userId, userName, lastUpdated) {
      //console.log(userId, userName + '' + lastUpdated);
      lock = true;

      // Create a new user object
      const newUser = {
        userId: userId,
        userName: userName,
        lastUpdated: lastUpdated,
      };
      console.log(newUser);

      // Combine the tables, add the new user, and sort alphabetically
      let combinedList = [...table1Rows.value, ...table2Rows.value];
      combinedList.push(newUser);

      combinedList.sort((a, b) => a.userName.localeCompare(b.userName));

      // Split the sorted list back into two tables
      const halfIndex = Math.ceil(combinedList.length / 2);
      table1Rows.value = combinedList.slice(0, halfIndex);
      table2Rows.value = combinedList.slice(halfIndex);

      lock = false;
    }
  );

// edit user
connection.on(
  'EditUser',
  function (userId, userName, statusName, note, lastUpdated) { // Include statusName
    //console.log(userId + '-' + userName + '-' + statusName + '-' + note + '-' + lastUpdated);
    lock = true;

      var user = table1Rows.value.find((x) => x.userId == userId);
      //console.log("Test1");
      //console.log(user);
      //console.log(table1Rows);
      if (user != null) {
        user.userName = userName;
        user.note = note;
        user.statusName = statusName;
        user.lastUpdated = lastUpdated;
      }

      user = table2Rows.value.find((x) => x.userId == userId);
      //console.log("Test2");
      //console.log(user);
      //console.log(table2Rows);
      if (user != null) {
        user.userName = userName;
        user.note = note;
        user.statusName = statusName;
        user.lastUpdated = lastUpdated;
      }
      lock = false;
  }
);


// edit user
connection.on(
  'DeleteUser',
  function (userId) { 

    lock = true;

    table1Rows.value = table1Rows.value.filter((x) => x.userId !== userId);

    table2Rows.value = table2Rows.value.filter((x) => x.userId !== userId);

    lock = false;
  }
);


  connection
    .start()
    .then(function () {
      console.log('SignalR Started');
    })
    .catch(function (err) {
      console.log("SignalR can't connect: " + err);
    });
}

</script>

<style>
.table-container {
  display: flex;
  gap: 50px; /* Space between the tables */
  width: 100%;
  height: calc(
    100vh - 64px
  ); /* Adjust height to fit within the viewport minus the header height */
  overflow: auto; /* Add scrolling if content overflows */
}

.q-page {
  width: 100%;
  height: 100%;
  overflow: auto; /* Add scrolling if content overflows */
}

.q-table {
  flex: 1; /* Make both tables take equal space */
  max-height: 100%; /* Ensure the table takes full height available */
  overflow: auto; /* Add scrolling within the table */
}

.q-table tbody td {
  font-size: calc(1.75vw);
  font-family: 'Arial';
  line-height: 1.4em;
}

.styled-table {
  border-collapse: collapse;
  font-size: 1em; /* Increased font size */
  font-family: Arial;
  min-width: 400px;
  background-color: #ffffff; /* White background */
}

.styled-table thead tr {
  background-color: #007bff;
  color: #ffffff;
  text-align: left;
  font-size: 1.3em; /* Increased font size */
}

.styled-table th {
  font-weight: bold; /* Make headers bold */
}

.styled-table th,
.styled-table td {
  padding: 14px 17px; /* Increased padding for larger font size */
  font-size: 1.6em; /* Make the text bigger */
}

.styled-table tbody tr {
  border-bottom: 1px solid #ffffff;
}

.styled-table tbody tr:nth-of-type(even) {
  background-color: #f3f3f3;
}

.styled-table tbody tr:last-of-type {
  border-bottom: 2px solid #ffffff;
}

body {
  overflow-y: hidden; /* Hide vertical scrollbar */
  background-color: #ffffff; /* White background */
}
</style>
