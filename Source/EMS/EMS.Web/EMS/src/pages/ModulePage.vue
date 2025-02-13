<template>
  <div class="text-center q-pa-md">
    <div class="col-6">
      <q-page class="q-pa-md flex flex-center">
        <q-card
          square
          class="full-height"
          style="max-width: 750px; width: 100%"
        >
            <q-card-section>
              <q-form class="q-px-sm q-pt-xl" @submit.prevent="submit">
                <h3 class="text-center shift-up">Module</h3>
                 <!-- Roles -->
              <q-select
                outlined
                v-model="roles"
                :options="roleOptions"
                option-label="name"
                option-value="id"
                label="Role"
                 lazy-rules
                :rules="[requiredRule('role')]"
                @update:model-value="updateFunctions"
              />

              <!-- Functions -->
              <q-select
                outlined
                v-model="functions"
                :options="functionOptions"
                option-label="moduleName"
                option-value="url"
                label="Functions"
                class="q-mt-md"
                 lazy-rules
                :rules="[requiredRule('function')]"
              />

              <!-- Continue button -->
              <q-btn
                unelevated
                size="lg"
                style="background-color: #1f459a"
                class="full-width text-white q-mt-md"
                type ="submit"
                label="Continue"
              />

            </q-form>
          </q-card-section>
        </q-card>
      </q-page>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'; 
import { useRouter } from 'vue-router'; 
import { useQuasar } from 'quasar'; 
import { appApi } from 'boot/axios'; 

const $q = useQuasar();
const router = useRouter(); 

const roles = ref(null);
const roleOptions = ref([]);

const functions = ref(null);
const functionOptions = ref([]);


const requiredRule = (fieldName: string) => (val: string) => !!val || `Please select a ${fieldName}.`;

const fetchRoles = async () => {
  try {
    const response = await appApi.get('api/Module/GetRoles');
    roleOptions.value = response.data;
   
  } catch (error) {
    $q.notify({
      color: 'negative',
      message: 'Failed to retrieve roles. Please contact admin.', 
    });
  }
};

const updateFunctions = async (role: { id: number; }) => {

  functionOptions.value = [];
  try {
    const response = await appApi.get(`api/Module/GetModulesByRole/${role.id}`);
    functionOptions.value = response.data; 
  } catch (error) {
    $q.notify({
      message: 'Failed to retrieve functions. Please contact admin.',
      color: 'negative',
    });
  }
};

const submit = () => {
  router.push(functions.value.url); // redirect to the URL
};

onMounted(fetchRoles);  
</script>