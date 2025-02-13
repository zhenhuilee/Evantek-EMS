<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <div class="q-pa-md">
        <q-option-group
          v-model="panel"
          inline
          :options="[
            { label: 'Open', value: 'open' },
            { label: 'Close', value: 'close' },
          ]"
        />

        <q-tab-panels v-model="panel">
          <!-- Open Incidents -->
          <q-tab-panel name="open">
            <q-table
              flat
              bordered
              :rows="rows1"
              :columns="columns1"
              row-key="id"
              :separator="separator1"
              :hide-pagination="true"
              :rows-per-page-options="[0]"
            >
              <template v-slot:body-cell-action="props">
                <q-td :props="props" align="left">
                  <q-btn color="blue-10" label="...">
                    <q-menu>
                      <q-list style="min-width: 100px">
                        <!-- Editing incident -->

                        <q-item clickable @click="goToEditPage(props.row.id)">
                          <q-item-section>Edit</q-item-section>
                        </q-item>

                        <!-- Deleting incident -->
                        <q-item
                          clickable
                          v-close-popup
                          @click="
                            openDeleteDialog(props.row.id, props.row.refNum)
                          "
                        >
                          <q-item-section>Delete</q-item-section>
                        </q-item>

                        <!-- View Signature -->
                        <q-item
                          clickable
                          @click="goToViewSignature(props.row.id)"
                        >
                          <q-item-section>View Signature</q-item-section>
                        </q-item>
                      </q-list>
                    </q-menu>
                  </q-btn>
                </q-td>
              </template>
            </q-table>
          </q-tab-panel>

          <!-- Close Incident -->
          <q-tab-panel name="close">
            <q-table
              flat
              bordered
              :rows="rows2"
              :columns="columns2"
              row-key="id"
              :separator="separator2"
              :hide-pagination="true"
              :rows-per-page-options="[0]"
            >
              <template v-slot:body-cell-action="props">
                <q-td :props="props" align="left">
                  <q-btn color="blue-10" label="...">
                    <q-menu>
                      <q-list style="min-width: 100px">
                        <!-- View Signature -->
                        <q-item
                          clickable
                          @click="goToViewSignature(props.row.id)"
                        >
                          <q-item-section>View Signature</q-item-section>
                        </q-item>
                      </q-list>
                    </q-menu>
                  </q-btn>
                </q-td>
              </template>
            </q-table>
          </q-tab-panel>
        </q-tab-panels>
      </div>
    </q-page-container>

    <q-dialog v-model="remove">
      <q-card style="min-width: 500px">
        <q-card-section>
          <q-form>
            <div class="row justify-end">
              <q-btn icon="close" flat round dense v-close-popup />
            </div>
            <br />
            <q-form>
              <h5>
                Are you sure you want to delete the incident with ref num
                {{ selectedRefNum }}?
              </h5>
            </q-form>

            <br />
            <q-btn
              unelevated
              size="lg"
              style="background-color: #1f459a"
              class="full-width text-white"
              @click="deleteBtn(selectedIncidentId)"
              label="Delete"
            />
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>
  </q-layout>
</template>

<style>
.shift-up {
  margin-top: -10px;
}
</style>

<script setup lang="ts">
import { ref, onMounted, Ref } from 'vue';
import { QTableProps, useQuasar } from 'quasar';
import { appApi } from 'boot/axios';
import { useRouter } from 'vue-router';

const router = useRouter();

const $q = useQuasar();

const panel = ref('open');

const goToEditPage = (id: number) => {
  router.push(`/admin/edit/${id}`);
};

const goToViewSignature = (id: number) => {
  router.push(`/admin/view-signature/${id}`);
};

const columns1: QTableProps['columns'] = [
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
    field: (row) => row.companyName,
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
    field: (row) => row.statusName,
    format: (val) => `${val}`,
    sortable: false,
  },
  {
    name: 'Subject',
    align: 'center',
    label: 'Subject',
    field: (row) => row.subjectName,
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
    field: (row) => row.engineerName,
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
    name: 'action',
    align: 'center',
    label: 'Action',
    field: 'action',
    sortable: false,
  },
];

const rows1 = ref([]);

const separator1: Ref<'cell' | 'horizontal' | 'vertical' | 'none'> =
  ref('cell');

async function fetchOpenIncident() {
  const response = await appApi.get('/api/Incidents/open-incidents');
  rows1.value = response.data;
}

const columns2: QTableProps['columns'] = [
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
    field: (row) => row.companyName,
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
    field: (row) => row.statusName,
    format: (val) => `${val}`,
    sortable: false,
  },
  {
    name: 'Subject',
    align: 'center',
    label: 'Subject',
    field: (row) => row.subjectName,
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
    field: (row) => row.engineerName,
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
    name: 'action',
    align: 'center',
    label: 'Action',
    field: 'action',
    sortable: false,
  },
];
const rows2 = ref([]);
const separator2: Ref<'cell' | 'horizontal' | 'vertical' | 'none'> =
  ref('cell');
async function fetchCloseIncident() {
  const response = await appApi.get('/api/Incidents/close-incidents');
  rows2.value = response.data;
}

onMounted(() => {
  fetchOpenIncident();
  fetchCloseIncident();
});

const remove = ref(false); // open or close the pop up
const selectedRefNum = ref('');
const selectedIncidentId = ref(0);

function openDeleteDialog(id: number, refNum: string) {
  selectedIncidentId.value = id; // open the pop up for the incident with that incident id
  selectedRefNum.value = refNum;
  remove.value = true;
}

const deleteBtn = async (incidentId: number) => {
  try {
    const response = await appApi.delete(`/api/Incidents/${incidentId}`);

    if (response.status === 200) {
      $q.notify({
        message: 'Incident deleted successfully.',
        color: 'positive',
      });

      await fetchOpenIncident();
      remove.value = false;
    } else {
      $q.notify({
        message: 'Failed to delete incident. Please try again.',
        color: 'negative',
      });
    }
  } catch (error) {
    console.error('Error deleting incident:', error);
    $q.notify({
      message: 'Failed to delete incident. Please try again.',
      color: 'negative',
    });
  }
};
</script>
