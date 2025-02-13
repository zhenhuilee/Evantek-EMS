<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <q-form class="q-px-sm q-pt-xl center-container" @submit.prevent="submit">
        <div class="q-pa-md form-container">
          <!-- Request Type-->
          <q-select
            outlined
            v-model="requestType"
            :options="requestTypeOptions"
            label="Request type"
            option-label="name"
            option-value="id"
            emit-value
            map-options
            class="q-mt-md"
            lazy-rules
            :rules="[(val: string | any[]) => !!val || 'Please select a request type.']"
          />

          <!-- Company -->
          <q-input
            outlined
            v-model="company"
            label="Company"
            class="q-mt-md"
            lazy-rules
            :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter the company.']"
          />

          <!-- Company type -->
          <q-select
            outlined
            v-model="companyType"
            :options="companyTypeOptions"
            label="Company type"
            option-label="name"
            option-value="id"
            emit-value
            map-options
            class="q-mt-md"
            lazy-rules
            :rules="[(val: string | any[]) => !!val || 'Please select a company type.']"
          />

          <!-- Sub Item -->
          <q-input
            outlined
            v-model="subItem"
            label="Sub Item (E.g. Clinic A/B/C, Branch A/B/C)"
            class="q-mt-md"
            lazy-rules
            :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter the sub item.']"
          />

          <!-- Customer-->
          <q-input
            outlined
            v-model="customer"
            label="Customer Name"
            class="q-mt-md"
            lazy-rules
            :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter the customer.']"
          />

          <!-- Customer Phone-->
          <q-input
            outlined
            v-model="customerPhone"
            label="Customer Phone"
            class="q-mt-md"
            lazy-rules
            :rules="[validatePhone]"
          />

          <!-- Subject -->
          <q-select
            outlined
            v-model="subjectType"
            :options="subjectTypeOptions"
            label="Subject"
            option-label="name"
            option-value="id"
            emit-value
            map-options
            class="q-mt-md"
            lazy-rules
            :rules="[(val) => !!val || 'Please select a subject.']"
          />

          <!-- Category -->
          <q-select
            outlined
            v-model="incidentCategory"
            :options="incidentCategoryOptions"
            label="Category"
            option-label="name"
            option-value="id"
            emit-value
            map-options
            class="q-mt-md"
            lazy-rules
            :rules="[(val) => !!val || 'Please select a category.']"
          />
          <!-- Description -->
          <q-input
            outlined
            v-model="description"
            label="Description"
            autogrow
            class="q-mt-md"
            lazy-rules
            :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter the description.']"
          />
          <!-- Address -->
          <q-input
            outlined
            v-model="address"
            label="Address"
            autogrow
            class="q-mt-md"
            lazy-rules
            :rules="[ (val: string | any[]) => val && val. length > 0 || 'Please enter the address.']"
          />
          <!-- IP Address-->
          <q-input
            outlined
            v-model="ipAddress"
            label="IP Address"
            class="q-mt-md"
            lazy-rules
            :rules="[validateIp]"
          />
          <!-- Engineer 
              <q-select
                 outlined 
                 v-model="engineer" 
                 :options="engineerOptions" 
                 label="Assigned to" 
                 option-label="name"
                 option-value="id" 
                 emit-value
                 map-options
                 class="q-mt-md"
                 lazy-rules
                 :rules="[(val) => !!val || 'Please select an engineer.']" 
                 />
              -->

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

          <!-- Work Order No-->
          <q-input
            outlined
            v-model="workOrderNo"
            label="Work Order No"
            class="q-mt-md"
          />
          <!-- Created By -->
          <q-select
            outlined
            v-model="admin"
            :options="adminOptions"
            label="Created By"
            option-label="name"
            option-value="id"
            emit-value
            map-options
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

          <!-- Submit Button -->
          <q-btn
            unelevated
            size="lg"
            color="blue-10"
            class="q-mt-md full-width text-white"
            type="submit"
            label="Submit"
          />
        </div>
      </q-form>
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useQuasar, date } from 'quasar';
import { appApi } from 'boot/axios';
import { useRouter } from 'vue-router';

const router = useRouter();

const $q = useQuasar();

const requestType = ref(null);
const requestTypeOptions = ref([]);

const companyType = ref(null);
const companyTypeOptions = ref([]);

const subjectType = ref(null);
const subjectTypeOptions = ref([]);

const incidentCategory = ref(null);
const incidentCategoryOptions = ref([]);

const engineer = ref(null); // id
const engineerOptions = ref([]);

const admin = ref(null);
const adminOptions = ref([]);

const workOrderNo = ref('');

const description = ref('');

const address = ref('');

const company = ref('');

const customer = ref('');

const customerPhone = ref('');

const ipAddress = ref('');

const responseDateTime = ref('');

const subItem = ref('');

const ipv4Pattern =
  /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
const ipv6Pattern = /^([0-9a-fA-F]{1,4}:){7}([0-9a-fA-F]{1,4}|:)$/;
const validateIp = (val: string) => {
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

const sgPhonePattern = /^[89]\d{3}\s?\d{4}$/;
const validatePhone = (val: string) => {
  if (!val || val.trim() === '') {
    return 'Please enter a phone number';
  } else if (!sgPhonePattern.test(val)) {
    return 'Please enter a valid phone number.';
  } else {
    return true;
  }
};

const fetchRequestType = async () => {
  try {
    const response = await appApi.get('api/Incidents/GetRequestType');
    requestTypeOptions.value = response.data;
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

onMounted(fetchRequestType);
onMounted(fetchCompanyType);
onMounted(fetchSubject);
onMounted(fetchIncidentCategory);
onMounted(fetchEngineers);
onMounted(fetchAdmin);

const submit = async () => {
  try {
    const userData = {
      requestTypeId: requestType.value, // <what you send in at the backend (refer to api)>/<v-model>.value
      company: company.value,
      companyTypeId: companyType.value,
      subItem: subItem.value,
      customer: customer.value,
      customerPhone: customerPhone.value,
      subjectId: subjectType.value,
      incidentCategoryId: incidentCategory.value,
      description: description.value,
      address: address.value,
      ipAddress: ipAddress.value ?? null,

      engineerId: engineer.value,

      //selectedStatus.value = statusOptions.value.find((element) => {
      //return element.value == data.statusId
      //});

      workOrderNo: workOrderNo.value ?? null,
      adminId: admin.value,
      responseDateTime: responseDateTime.value
        ? date.extractDate(responseDateTime.value, 'YYYY-MM-DD HH:mm') // date.extractDate() is called to convert the string into a Date object.
        : null,
    };

    const response = await appApi.post('/api/Incidents', userData);

    if (response.status == 200) {
      $q.notify({
        color: 'positive',
        message: 'Incident added successfully.',
      });
      router.push('/admin/all-incidents');
    } else {
      console.error('Failed to add incident, status code:', response.status);
    }
  } catch (error) {
    console.error('Error adding incident:', error);
    $q.notify({
      message: 'Failed to add incident. Please try again.',
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
