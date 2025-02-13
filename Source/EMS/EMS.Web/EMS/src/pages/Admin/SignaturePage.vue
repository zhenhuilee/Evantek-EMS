<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <q-form class="q-px-sm q-pt-xl center-container">
        <div class="q-pa-md form-container">
          <!-- Ref Num -->
          <q-input
            outlined
            v-model="refNum"
            label="Reference Number"
            readonly
          />
          <br />

          <!-- Status -->
          <q-select
            outlined
            v-model="incidentStatus"
            :options="incidentStatusOptions"
            label="Status"
            option-value="value"
            option-label="name"
            readonly
          />
          <br />

          <!-- Request Type-->
          <q-select
            outlined
            v-model="requestType"
            :options="requestTypeOptions"
            label="Request type"
            option-value="value"
            option-label="name"
            readonly
          />
          <br />

          <!-- Company -->
          <q-input outlined v-model="company" label="Company" />
          <br />

          <!-- Company type -->
          <q-select
            outlined
            v-model="companyType"
            :options="companyTypeOptions"
            label="Company type"
            option-label="name"
            option-value="value"
            readonly
          />
          <br />

          <!-- Sub Item -->
          <q-input
            outlined
            v-model="subItem"
            label="Sub Item (E.g. Clinic A/B/C, Branch A/B/C)"
            readonly
          />
          <br />

          <!-- Customer Nme-->
          <q-input outlined v-model="customer" label="Customer" readonly />
          <br />

          <!-- Customer Phone-->
          <q-input
            outlined
            v-model="customerPhone"
            label="Customer Phone"
            readonly
          />
          <br />

          <!-- Subject -->
          <q-select
            outlined
            v-model="subjectType"
            :options="subjectTypeOptions"
            label="Subject"
            option-label="name"
            option-value="id"
            readonly
          />
          <br />

          <!-- Category -->
          <q-select
            outlined
            v-model="incidentCategory"
            :options="incidentCategoryOptions"
            label="Category"
            option-label="name"
            option-value="id"
            readonly
          />
          <br />

          <!-- Description -->
          <q-input
            outlined
            v-model="description"
            label="Description"
            autogrow
            readonly
          />
          <br />

          <!-- Address -->
          <q-input
            outlined
            v-model="address"
            label="Address"
            autogrow
            readonly
          />
          <br />

          <!-- IP Address-->
          <q-input outlined v-model="ipAddress" label="IP Address" readonly />
          <br />

          <!-- Engineer -->
          <q-select
            outlined
            v-model="engineer"
            :options="engineerOptions"
            label="Assigned to"
            readonly
            multiple
            option-label="name"
            option-value="id"
            emit-value
            map-options
            class="q-mt-md"
          />
          <br />

          <!-- Work Order No-->
          <q-input
            outlined
            v-model="workOrderNo"
            label="Work Order No"
            readonly
          />
          <br />

          <!-- Created By -->
          <q-select
            outlined
            v-model="admin"
            :options="adminOptions"
            label="Created By"
            option-label="name"
            option-value="id"
            class="q-mt-md"
            readonly
          />
          <br />

          <!--Response Date and time-->
          <q-input
            outlined
            v-model="responseDateTime"
            label="Response Date & Time"
            readonly
          >
            <!--calender-->
            <template v-slot:prepend>
              <q-icon name="event" class="cursor-pointer">
                <q-popup-proxy
                  cover
                  transition-show="scale"
                  transition-hide="scale"
                >
                  <q-date v-model="responseDateTime" mask="YYYY-MM-DD HH:mm">
                    <div class="row items-center justify-end">
                      <q-btn v-close-popup label="Close" color="primary" flat />
                    </div>
                  </q-date>
                </q-popup-proxy>
              </q-icon>
            </template>

            <!-- time -->
            <template v-slot:append>
              <q-icon name="access_time" class="cursor-pointer">
                <q-popup-proxy
                  cover
                  transition-show="scale"
                  transition-hide="scale"
                >
                  <q-time
                    v-model="responseDateTime"
                    mask="YYYY-MM-DD HH:mm"
                    format24h
                  >
                    <div class="row items-center justify-end">
                      <q-btn v-close-popup label="Close" color="primary" flat />
                    </div>
                  </q-time>
                </q-popup-proxy>
              </q-icon>
            </template>
          </q-input>
          <br />

          <!--Completed date and time -->
          <q-input
            outlined
            v-model="completedDateTime"
            label="Completed Date & Time"
            readonly
          >
            <template v-slot:prepend>
              <q-icon name="event" class="cursor-pointer">
                <q-popup-proxy
                  cover
                  transition-show="scale"
                  transition-hide="scale"
                >
                  <q-date v-model="completedDateTime" mask="YYYY-MM-DD HH:mm">
                    <div class="row items-center justify-end">
                      <q-btn v-close-popup label="Close" color="primary" flat />
                    </div>
                  </q-date>
                </q-popup-proxy>
              </q-icon>
            </template>
            <template v-slot:append>
              <q-icon name="access_time" class="cursor-pointer">
                <q-popup-proxy
                  cover
                  transition-show="scale"
                  transition-hide="scale"
                >
                  <q-time
                    v-model="completedDateTime"
                    mask="YYYY-MM-DD HH:mm"
                    format24h
                  >
                    <div class="row items-center justify-end">
                      <q-btn v-close-popup label="Close" color="primary" flat />
                    </div>
                  </q-time>
                </q-popup-proxy>
              </q-icon>
            </template>
          </q-input>
          <br />

          <!--Arrival date and time -->
          <q-input
            outlined
            v-model="arrivalDateTime"
            label="Arrival Date & Time"
            readonly
          >
            <template v-slot:prepend>
              <q-icon name="event" class="cursor-pointer">
                <q-popup-proxy
                  cover
                  transition-show="scale"
                  transition-hide="scale"
                >
                  <q-date v-model="arrivalDateTime" mask="YYYY-MM-DD HH:mm">
                    <div class="row items-center justify-end">
                      <q-btn v-close-popup label="Close" color="primary" flat />
                    </div>
                  </q-date>
                </q-popup-proxy>
              </q-icon>
            </template>
            <template v-slot:append>
              <q-icon name="access_time" class="cursor-pointer">
                <q-popup-proxy
                  cover
                  transition-show="scale"
                  transition-hide="scale"
                >
                  <q-time
                    v-model="arrivalDateTime"
                    mask="YYYY-MM-DD HH:mm"
                    format24h
                  >
                    <div class="row items-center justify-end">
                      <q-btn v-close-popup label="Close" color="primary" flat />
                    </div>
                  </q-time>
                </q-popup-proxy>
              </q-icon>
            </template>
          </q-input>
          <br />

          <!-- Solution -->
          <q-input
            outlined
            v-model="solution"
            label="Solution"
            autogrow
            readonly
          />
          <br />

          <!-- Replacement table -->
          <q-table
            flat
            bordered
            :rows="rows"
            :columns="columns"
            row-key="id"
            :separator="separator"
            style="width: 450px"
            v-model="replacementTable"
            :hide-pagination="true"
            :rows-per-page-options="[0]"
            readonly
          >
            <template v-slot:top>
              <div class="col-2 q-table__title">Replacement</div>
              <q-btn
                class="q-ml-auto text-white q-pa-sm"
                label="Add rows"
                style="background-color: #1f459a"
                dense
                icon="add"
                @click="addRow"
              />
            </template>

            <template v-slot:body-cell="props">
              <q-td :props="props">
                <q-input
                  v-model="props.row[props.col.name]"
                  input-class="text-right"
                  dense
                  borderless
                />
              </q-td>
            </template>
          </q-table>

          <br />

          <q-img :src="url" style="height: 200px; width: 300px" />

          <br />
        </div>
      </q-form>
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref, onMounted, Ref } from 'vue';
import { date, QTableProps, useQuasar } from 'quasar';
import { appApi } from 'boot/axios';
import { useRoute } from 'vue-router';

const route = useRoute();

const separator: Ref<'cell' | 'horizontal' | 'vertical' | 'none'> = ref('cell');

const replacementTable = ref([]);

const rows = ref([]);

const columns: QTableProps['columns'] = [
  {
    name: 'model',
    align: 'center',
    label: 'Model',
    field: (row) => row.model,
    format: (val: string) => `${val}`,
    sortable: false,
  },
  {
    name: 'oldSerialNo',
    align: 'center',
    label: 'Old Serial No',
    field: (row) => row.oldSerialNo,
    format: (val: string) => `${val}`,
    sortable: false,
  },
  {
    name: 'newSerialNo',
    align: 'center',
    label: 'New Serial No',
    field: (row) => row.newSerialNo,
    format: (val: string) => `${val}`,
    sortable: false,
  },
  {
    name: 'remarks',
    align: 'center',
    label: 'Remarks',
    field: (row) => row.remarks,
    format: (val: string) => `${val}`,
    sortable: false,
  },
];

const addRow = () => {
  rows.value.push({ model: '', oldSerialNo: '', newSerialNo: '', remarks: '' });
};

const $q = useQuasar();
const refNum = ref(null); // v-model

const incidentStatus = ref(null);
const incidentStatusOptions = ref([]);

const company = ref('');

const requestType = ref(null);
const requestTypeOptions = ref([]);

const companyType = ref(null);
const companyTypeOptions = ref([]);

const subjectType = ref(null);
const subjectTypeOptions = ref([]);

const incidentCategory = ref(null);
const incidentCategoryOptions = ref([]);

const engineer = ref(null);
const engineerOptions = ref([]);

const admin = ref(null);
const adminOptions = ref([]);

const workOrderNo = ref('');
const description = ref('');
const address = ref('');
const customer = ref('');
const customerPhone = ref('');
const ipAddress = ref('');
const responseDateTime = ref('');
const completedDateTime = ref('');
const arrivalDateTime = ref('');
const subItem = ref('');
const solution = ref('');

const url = ref('');

const fetchRequestType = async () => {
  try {
    const response = await appApi.get('api/Incidents/GetRequestType');
    requestTypeOptions.value = response.data;
    console.log(response);
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve incident type. Please contact admin.',
      color: 'negative',
    });
  }
};

const fetchCompanyType = async () => {
  try {
    const response = await appApi.get('api/Incidents/GetCompanyType');
    companyTypeOptions.value = response.data;
    console.log(response);
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve company type. Please contact admin.',
      color: 'negative',
    });
  }
};

const fetchSubject = async () => {
  try {
    const response = await appApi.get('api/Incidents/GetSubjectList');
    subjectTypeOptions.value = response.data;
    console.log(response);
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve subject. Please contact admin.',
      color: 'negative',
    });
  }
};

const fetchIncidentCategory = async () => {
  try {
    const response = await appApi.get('api/Incidents/GetIncidentCategory');
    incidentCategoryOptions.value = response.data;
    console.log(response);
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve incident category. Please contact admin.',
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

const fetchAdmin = async () => {
  try {
    const response = await appApi.get('/api/Incidents/GeAdminList');
    adminOptions.value = response.data;
    console.log(response);
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve admin. Please contact admin.',
      color: 'negative',
    });
  }
};

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

const fetchIncidentDetails = async () => {
  try {
    const response = await appApi.get(
      `/api/Incidents/EngineerGetIncidentDetails/${route.params.id}`
    );
    const data = response.data;

    refNum.value = data.refNum;

    incidentStatus.value = incidentStatusOptions.value.find((element) => {
      return element.id == data.statusId;
    });

    requestType.value = requestTypeOptions.value.find((element) => {
      return element.id == data.requestTypeId;
    });

    companyType.value = companyTypeOptions.value.find((element) => {
      return element.id == data.companyTypeId;
    });

    company.value = data.company;

    subItem.value = data.subItem;

    customer.value = data.customer;

    customerPhone.value = data.customerPhone;

    subjectType.value = subjectTypeOptions.value.find((element) => {
      return element.id == data.subjectId;
    });

    incidentCategory.value = incidentCategoryOptions.value.find((element) => {
      return element.id == data.categoryId;
    });

    admin.value = adminOptions.value.find((element) => {
      return element.id == data.adminId;
    });

    /*
    engineer.value = engineerOptions.value.find((element) => {
      return element.id == data.engineerId;
    });
    */

    engineer.value = engineerOptions.value.filter((element) =>
      data.engineerId.includes(element.id)
    );

    description.value = data.description;

    address.value = data.address;

    ipAddress.value = data.ipAddress;

    workOrderNo.value = data.workOrderNo;

    responseDateTime.value = data.responseDateTime
      ? date.formatDate(data.responseDateTime, 'YYYY-MM-DD HH:mm')
      : null;

    completedDateTime.value = data.completedDateTime
      ? date.formatDate(data.completedDateTime, 'YYYY-MM-DD HH:mm')
      : null;

    arrivalDateTime.value = data.arrivalDateTime
      ? date.formatDate(data.arrivalDateTime, 'YYYY-MM-DD HH:mm')
      : null;

    solution.value = data.solution;

    if (Array.isArray(data.replacements)) {
      data.replacements.forEach(
        (replacement: {
          model: string;
          oldSerialNo: string;
          newSerialNo: string;
          remarks: string;
        }) => {
          rows.value.push({
            model: replacement.model,
            oldSerialNo: replacement.oldSerialNo,
            newSerialNo: replacement.newSerialNo,
            remarks: replacement.remarks,
          });
        }
      );
    }
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve incident details. Please contact admin.',
      color: 'negative',
    });
    console.error('Error fetching incident details:', error);
  }
};

const fetchSignature = async () => {
  const response = await appApi.get(
    `/api/Incidents/signature/${route.params.id}`
  );
  url.value = response.data; // response.data = data url --> passing it to url.value
  console.log(response.data);
};

onMounted(fetchRequestType);
onMounted(fetchCompanyType);
onMounted(fetchSubject);
onMounted(fetchIncidentCategory);
onMounted(fetchEngineers);
onMounted(fetchAdmin);
onMounted(fetchIncidentStatus);
onMounted(fetchIncidentDetails);
onMounted(fetchSignature);
</script>
<style>
.center-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh; /* Ensures the form is vertically centered */
}
.form-container {
  width: 100%;
  max-width: 450px;
}
</style>
