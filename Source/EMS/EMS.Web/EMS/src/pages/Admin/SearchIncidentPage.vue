<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <q-form class="q-px-sm q-pt-xl left">
        <div class="q-pa-md form-container">
          <q-input
            v-model="fromDate"
            outlined
            type="date"
            label="From date"
            class="fixed-width"
          />

          <br />

          <q-input
            v-model="toDate"
            outlined
            type="date"
            label="To date"
            class="fixed-width"
          />

          <br />

          <q-select
            outlined
            v-model="incidentStatus"
            :options="incidentStatusOptions"
            label="Status"
            option-value="value"
            option-label="name"
          />

          <br />

          <!--<q-select 
          outlined 
          v-model="engineer" 
          :options="engineerOptions" 
          label="Engineer" 
          option-label="name"
          option-value="id" 
        
        />-->

          <q-select
            outlined
            v-model="engineer"
            :options="engineerOptions"
            label="Assigned To"
            multiple
            option-label="name"
            option-value="id"
            emit-value
            map-options
          >
            <template
              v-slot:option="{ itemProps, opt, selected, toggleOption }"
            >
              <q-item v-bind="itemProps">
                <q-item-section>
                  <q-item-label>{{ opt.name }}</q-item-label>
                  <!-- Ensure opt.name exists -->
                </q-item-section>
                <q-item-section side>
                  <q-toggle
                    :model-value="selected"
                    @update:model-value="toggleOption(opt)"
                  />
                </q-item-section>
              </q-item>
            </template>
          </q-select>

          <br />

          <div class="row justify-center q-mt-md">
            <div class="col-6">
              <q-btn
                unelevated
                size="lg"
                color="black"
                class="full-width text-white"
                type="submit"
                label="Search"
                @click.prevent="searchResult()"
              />
            </div>
          </div>
        </div>
      </q-form>

      <q-table
        flat
        bordered
        title="Incidents"
        :rows="rows"
        :columns="columns"
        color="primary"
        row-key="id"
        :separator="separator"
      >
        <template v-slot:top-right>
          <q-btn
            color="green"
            icon-right="archive"
            label="Export to Excel"
            no-caps
            @click="downloadExcel()"
          />
        </template>
      </q-table>
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref, onMounted, Ref } from 'vue';
import { exportFile, QTableProps, useQuasar } from 'quasar';
import { appApi } from 'boot/axios';

const $q = useQuasar();

const fromDate = ref('');
const toDate = ref('');

const incidentStatus = ref(null);
const incidentStatusOptions = ref([]);

const engineer = ref(null);
const engineerOptions = ref([]);

const separator: Ref<'cell' | 'horizontal' | 'vertical' | 'none'> = ref('cell');

const rows = ref([]);

const columns: QTableProps['columns'] = [
  {
    name: 'refNum',
    required: true,
    label: 'Ref Num',
    align: 'center',
    field: (row) => row.refNum,
    sortable: false,
  },
  {
    name: 'Company',
    required: true,
    label: 'Company',
    align: 'center',
    field: (row) => row.company,
    format: (val) => `${val}`,
    sortable: false,
  },
  {
    name: 'Sub Item',
    required: true,
    label: 'Sub Item (E.g. Clinic A/B/C, Branch A/B/C)',
    align: 'center',
    field: (row) => row.subItem,
    format: (val) => `${val}`,
    sortable: false,
  },
  {
    name: 'Status',
    align: 'center',
    label: 'Status',
    field: (row) => row.status,
    format: (val) => `${val}`,
    sortable: false,
  },
  {
    name: 'IP',
    align: 'center',
    label: 'IP Address',
    field: (row) => row.ipAddress,
    format: (val) => (val == null ? '' : `${val}`), // Handle null by returning an empty string
    sortable: false,
  },
  {
    name: 'Engineer',
    align: 'center',
    label: 'Engineer',
    field: (row) => row.engineer,
    format: (val: string) => `${val}`,
    sortable: false,
  },
  {
    name: 'Date',
    align: 'center',
    label: 'Date created',
    field: (row) => row.incidentCreatedDateTime,
    format: (val: string) => {
      const date = new Date(val); // Convert the string to a Date object
      const day = date.getDate(); // Get day of the month
      const month = date.toLocaleString('en-US', { month: 'short' }); // Get abbreviated month name
      const year = date.getFullYear(); // Get full year
      return `${day} ${month} ${year}`; // Format as "dd MMMM yyyy"
    },
    sortable: false,
  },
  {
    name: 'Description',
    required: true,
    label: 'Description',
    align: 'center',
    field: (row) => row.description,
    format: (val) => `${val}`,
    sortable: false,
  },
  {
    name: 'Solution',
    required: true,
    label: 'Solution',
    align: 'center',
    field: (row) => row.solution,
    format: (val) => (val == null ? '' : `${val}`),
    sortable: false,
  },
];

const fetchIncidentStatus = async () => {
  try {
    const response = await appApi.get('api/Incidents/GetIncidentStatus');

    incidentStatusOptions.value = response.data;
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve incident status. Please contact admin.',
      color: 'negative',
    });
  }
};

const fetchEngineers = async () => {
  try {
    const response = await appApi.get('api/Incidents/GetEngineerList');
    engineerOptions.value = response.data;
    console.log(response);
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve engineers Please contact admin.',
      color: 'negative',
    });
  }
};

const searchResult = async () => {
  try {
    const payload = {
      fromDate: new Date(fromDate.value),
      toDate: new Date(toDate.value),
      statusId: incidentStatus.value?.id || null,
      //engineerId: engineer.value?.id || null,
      engineerIds: engineer.value || [],
    };

    const response = await appApi.post(
      'api/Incidents/GetFilteredIncidents',
      payload
    );

    rows.value = response.data;
  } catch (error) {
    $q.notify({
      message: 'Failed to fetch incidents. Please try again later.',
      color: 'negative',
      icon: 'warning',
    });
  }
};

async function downloadExcel() {
  try {
    const response = await appApi.post(
      'api/Incidents/GetIncReportExcel',
      rows.value,
      { responseType: 'blob' }
    );

    let fileName = 'Incident Report.xlsx';
    const disposition = response.headers['content-disposition'];
    if (disposition && disposition.indexOf('attachment') !== -1) {
      var filenameRegex = /filename[^;=\n]=((['"]).?\2|[^;\n]*)/;
      var matches = filenameRegex.exec(disposition);
      if (matches != null && matches[1]) {
        fileName = matches[1].replace(/['"]/g, '');
      }
    }

    const result = response.data;
    CreateDownloadPopup(result, fileName);
  } catch (error) {
    $q.notify({
      message: 'Failed to download excel. Please try again later.',
      color: 'negative',
    });
  }
}

async function CreateDownloadPopup(
  datablob: any,
  fileName: string
): Promise<void> {
  const href = URL.createObjectURL(datablob);

  // create "a" HTML element with href to file & click
  const link = document.createElement('a');
  link.href = href;
  link.setAttribute('download', fileName); //or any other extension
  document.body.appendChild(link);
  link.click();

  // clean up "a" element & remove ObjectURL
  document.body.removeChild(link);
  URL.revokeObjectURL(href);
}

onMounted(() => {
  fetchIncidentStatus();
  fetchEngineers();
});
</script>
