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
                        <!--View incident-->
                        <q-item
                          clickable
                          @click="goToViewIncident(props.row.id)"
                        >
                          <q-item-section>View</q-item-section>
                        </q-item>

                        <!--Edit incident-->
                        <q-item
                          clickable
                          @click="goToEditIncidentPage(props.row.id)"
                        >
                          <q-item-section>Edit</q-item-section>
                        </q-item>

                        <!--Signature -->
                        <q-item
                          clickable
                          @click="goToSignaturePage(props.row.id)"
                        >
                          <q-item-section>Signature</q-item-section>
                        </q-item>
                      </q-list>
                    </q-menu>
                  </q-btn>
                </q-td>
              </template>
            </q-table>
          </q-tab-panel>

          <!-- Close Incidents -->
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
                        <!--Signature -->
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
  </q-layout>
</template>

<style>
.shift-up {
  margin-top: -10px;
}
</style>

<script setup lang="ts">
import { ref, onMounted, Ref } from 'vue';
import { QTableProps } from 'quasar';
import { appApi } from 'boot/axios';
import { useRouter } from 'vue-router';

const router = useRouter();

const panel = ref('open');

const goToEditIncidentPage = (id: number) => {
  router.push(`/engineer/edit-incidents/${id}`);
};

const goToSignaturePage = (id: number) => {
  router.push(`/engineer/signature/${id}`);
};

const goToViewIncident = (id: number) => {
  router.push(`/engineer/view-incidents/${id}`);
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
    field: (row) => row.company,
    format: (val) => `${val}`,
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
  const response = await appApi.get(
    '/api/Incidents/GetOpenIncidentsByEngineer'
  );
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
    field: (row) => row.company,
    format: (val) => `${val}`,
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
  const response = await appApi.get(
    '/api/Incidents/GetCloseIncidentsByEngineer'
  );
  rows2.value = response.data;
}

onMounted(() => {
  fetchOpenIncident();
  fetchCloseIncident();
});
</script>
