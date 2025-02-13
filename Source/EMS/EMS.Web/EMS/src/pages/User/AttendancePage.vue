<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <q-page class="q-pa-md flex flex-center">
        
        <q-card
          square
          class="full-height"
          style="max-width: 750px; width: 100%"
        >
            <q-card-section>
              <q-form class="q-px-sm q-pt-xl" @submit.prevent="submit">
                <h3 class="text-center shift-up">Update attendance</h3>
                <!-- Status dropdown-->
                <q-select
                  outlined
                  v-model="selectedStatus" 
                  :options="statusOptions"
                  label="Status"
                  :loading="statusLoading"
                  :disable="statusLoading"   
                  option-value="value"
                  option-label="name" 
                />
                <br />
                <!-- Note-->
                <q-input
                  outlined
                  v-model="note"
                  label="Note"
                  autogrow
                  lazy-rules
                :rules="[(val: string) => val.length <= 50 || 'Note cannot exceed 50 characters.']"
                
                />
                <br/>
                <!-- Update attendance button-->
                <q-btn 
                unelevated
                size="lg"
                style="background-color: #1f459a"
                class="full-width text-white"
                type="submit"
                label="update attendance"
              />
              </q-form>
              
            </q-card-section>
        </q-card>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

<style>
.shift-up {
  margin-top: -10px; 
}
</style>

<script setup lang="ts">
import { ref, onMounted} from 'vue';
import { useQuasar } from 'quasar';
import { appApi } from 'boot/axios';

const $q = useQuasar();

const selectedStatus = ref(null);
const statusLoading = ref(true);
const statusOptions = ref([]);

const note = ref('');


class Option {
  name: string;
  value: number;
  isCategory: boolean;
}

const fetchStatusOptions = async ()  => {
    try {
    const response = await appApi.get('api/Attendance/GetStatuslist');

    statusOptions.value.length = 0;

    response.data.forEach(
      (element: {
        categoryName: string;
        statuses: Array<{ id: number; name: string }>;
      }) => {
        element.statuses.forEach((status: { id: number; name: string }) => {
          var statusOption: Option = {
            name: `${element.categoryName} - ${status.name}`,
            value: status.id,
            isCategory: false,
          };
          statusOptions.value.push(statusOption);
        });
      }
    );
    statusLoading.value = false;
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve status list. Please contact admin.',
      color: 'negative',
    });
  }
}

const fetchUserAttendance = async () => {
  try {
    const response = await appApi.get('api/Attendance/GetUserAttendance');
    const data = response.data;

    selectedStatus.value = statusOptions.value.find((element) => {
      return element.value == data.statusId 
    });
    console.log(selectedStatus.value)

    note.value = data.note;
  } catch (error) {
    //console.log(error);
    $q.notify({
      message: 'Failed to retrieve user attendance. Please contact admin.',
      color: 'negative',
    });
  } 
}
  
const submit = async () => {
  try {
    await appApi.put('api/Attendance/UpdateAttendance', {
      note: note.value,
      statusId: selectedStatus.value.value,
    });
    $q.notify({
      color: 'positive',
      message: 'Attendance updated successfully.',
      
    });
  } catch (error) {
    //console.log(error);
    $q.notify({
      color: 'negative',
      message: 'Failed to update attendance. Please contact admin.',
      
    });
  }
};

onMounted(async () => {
  await fetchStatusOptions();
  await fetchUserAttendance();
});


</script>