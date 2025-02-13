<template>
  <q-layout view="hHh lpR fFf">
    <!--<div class="row justify-end q-pa-md">
      <q-btn round color="green" icon="download" />
    </div>-->

    <q-page-container>
      <q-form class="q-px-sm q-pt-xl center-container" @submit.prevent="submit">
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
            lazy-rules
            :rules="[(val: string | any[]) => !!val || 'Please select a status.']"
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
            lazy-rules
            :rules="[(val: string | any[]) => !!val || 'Please select a request type.']"
          />
          <br />

          <!-- Company -->
          <q-input
            outlined
            v-model="company"
            label="Company"
            lazy-rules
            :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter the company.']"
          />
          <br />

          <!-- Company type -->
          <q-select
            outlined
            v-model="companyType"
            :options="companyTypeOptions"
            label="Company type"
            option-label="name"
            option-value="value"
            lazy-rules
            :rules="[(val: string | any[]) => !!val || 'Please select a company type.']"
          />
          <br />

          <!-- Sub Item -->
          <q-input
            outlined
            v-model="subItem"
            label="Sub Item (E.g. Clinic A/B/C, Branch A/B/C)"
            lazy-rules
            :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter the sub item.']"
          />
          <br />

          <!-- Customer Nme-->
          <q-input
            outlined
            v-model="customer"
            label="Customer"
            lazy-rules
            :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter the customer.']"
          />
          <br />

          <!-- Customer Phone-->
          <q-input
            outlined
            v-model="customerPhone"
            label="Customer Phone"
            lazy-rules
            :rules="[validatePhone]"
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
            lazy-rules
            :rules="[(val) => !!val || 'Please select a subject.']"
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
            lazy-rules
            :rules="[(val) => !!val || 'Please select a category.']"
          />
          <br />

          <!-- Description -->
          <q-input
            outlined
            v-model="description"
            label="Description"
            autogrow
            lazy-rules
            :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter the description.']"
          />
          <br />

          <!-- Address -->
          <q-input
            outlined
            v-model="address"
            label="Address"
            autogrow
            lazy-rules
            :rules="[ (val: string | any[]) => val && val. length > 0 || 'Please enter the address.']"
          />
          <br />

          <!-- IP Address-->
          <q-input
            outlined
            v-model="ipAddress"
            label="IP Address"
            lazy-rules
            :rules="[validateIp]"
          />
          <br />

          <!-- Engineer -->
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
            class="q-mt-md"
            lazy-rules
            :rules="[(val) => !!val || 'Please select an engineer.']"
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

          <!-- Work Order No-->
          <q-input outlined v-model="workOrderNo" label="Work Order No" />
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
            lazy-rules
            :rules="[(val) => !!val || 'Please select an admin.']"
          />
          <br />

          <!--Response Date and time-->
          <q-input
            outlined
            v-model="responseDateTime"
            label="Response Date & Time"
          >
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

          <!-- Submit Button -->
          <q-btn
            unelevated
            size="lg"
            color="blue-10"
            class="full-width text-white"
            type="submit"
            label="Edit"
          />
        </div>
      </q-form>
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { date, format, useQuasar } from 'quasar';
import { appApi } from 'boot/axios';
import { useRoute } from 'vue-router';
import { useRouter } from 'vue-router';

const router = useRouter();
const route = useRoute();
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
const subItem = ref('');

const validateIp = (val: string) => {
  const ipv4Pattern =
    /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
  const ipv6Pattern = /^([0-9a-fA-F]{1,4}:){7}([0-9a-fA-F]{1,4}|:)$/;

  if (
    val == '' ||
    val == null ||
    ipv4Pattern.test(val) ||
    ipv6Pattern.test(val)
  ) {
    return true;
  } else {
    return 'Please enter a valid IP address (IPv4 or IPv6).';
  }
};

const validatePhone = (val: string) => {
  const phonePattern =
    /^(\+?\d{1,4}?[-.\s]?)?((\(\d{1,3}\))|\d{1,4})[-.\s]?\d{1,4}[-.\s]?\d{1,9}$/;

  if (phonePattern.test(val)) {
    return true;
  } else {
    return 'Please enter a valid phone number.';
  }
};

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
    const response = await appApi.get(`/api/Incidents/${route.params.id}`);
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
  } catch {
    $q.notify({
      color: 'negative',
      message: 'Failed to retrieve incident details.',
    });
  }
};

onMounted(fetchRequestType);
onMounted(fetchCompanyType);
onMounted(fetchSubject);
onMounted(fetchIncidentCategory);
onMounted(fetchEngineers);
onMounted(fetchAdmin);
onMounted(fetchIncidentStatus);
onMounted(fetchIncidentDetails);

const submit = async () => {
  try {
    const editInfo = {
      id: route.params.id,

      refNum: refNum.value,

      statusId: incidentStatus.value.id,

      requestTypeId: requestType.value.id,

      company: company.value,

      companyTypeId: companyType.value.id,

      subItem: subItem.value,

      customer: customer.value,

      customerPhone: customerPhone.value,

      subjectId: subjectType.value.id,

      categoryId: incidentCategory.value.id,

      adminId: admin.value.id,

      engineerId: engineer.value.map((engineer: { id: number }) =>
        typeof engineer === 'object' ? engineer.id : engineer
      ),

      description: description.value,

      address: address.value,

      ipAddress: ipAddress.value,

      workOrderNo: workOrderNo.value,

      responseDateTime: responseDateTime.value
        ? date.extractDate(responseDateTime.value, 'YYYY-MM-DD HH:mm')
        : null,
    };
    console.log('responseDateTime:', responseDateTime.value);

    const response = await appApi.put('/api/Incidents/edit', editInfo);

    if (response.status == 200) {
      $q.notify({
        message: 'Incident edited successfully.',
        color: 'positive',
      });
      router.push('/admin/all-incidents');
    }
  } catch {
    $q.notify({
      message: 'Failed to edit incident.',
      color: 'negative',
    });
  }
};
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
