<template>
  <!--  <div id="signature-pad" class="signature-pad">
    <div class="signature-pad--body">
      <canvas></canvas>
    </div>

    <div class="signature-pad--footer">
      <div class="description">Sign above</div>

      <div class="signature-pad--actions">
        <div>
          <q-btn type="button" class="button clear" @click="clear">
            Clear
          </q-btn>
          <button type="button" class="button" data-action="change-color">
            Change color
          </button>
          <button type="button" class="button" data-action="undo">Undo</button>
        </div>

        <div>
          <q-btn type="button" class="button save" @click="getData()">
            Save as PNG
          </q-btn>
          <button type="button" class="button save" data-action="save-jpg">
            Save as JPG
          </button>
          <button type="button" class="button save" data-action="save-svg">
            Save as SVG
          </button>
        </div>
      </div>
    </div>
  </div>-->

  <div class="text-center q-pa-md">
    <div class="col-6">
      <q-page class="q-pa-md flex flex-center">
        <q-card
          square
          class="full-height"
          style="max-width: 750px; width: 100%"
        >
          <q-card-section>
            <img
              class="text-center"
              src="/images/evanteklogo.jpeg"
              alt="Logo"
              style="max-width: 240px"
            />
            <h5>EMPLOYEE MANAGEMENT SYSTEM (EMS)</h5>
            <q-form class="q-px-sm q-pt-xl" @submit.prevent="submit">
              <!-- Username -->
              <q-input
                outlined
                v-model="loginName"
                label="Username"
                lazy-rules
                :rules="[requiredRule('username')]"
              >
                <template v-slot:prepend>
                  <q-icon name="person" />
                </template>
              </q-input>

              <!-- Password -->
              <q-input
                v-model="password"
                outlined
                label="Password"
                :type="isPwd ? 'password' : 'text'"
                lazy-rules
                :rules="[requiredRule('password')]"
                class="q-mt-md"
              >
                <template v-slot:prepend>
                  <q-icon name="lock" />
                </template>

                <template v-slot:append>
                  <q-icon
                    :name="isPwd ? 'visibility_off' : 'visibility'"
                    class="cursor-pointer"
                    @click="togglePasswordVisibility"
                  />
                </template>
              </q-input>

              <!-- Checkbox -->
              <q-checkbox
                size="md"
                v-model="rememberMe"
                label="Stay Signed In?"
                val="md"
                class="q-mt-md"
              />

              <!-- Login btn -->
              <q-btn
                unelevated
                size="lg"
                style="background-color: #1f459a"
                class="full-width text-white q-mt-md"
                label="LOGIN"
                type="submit"
              />
            </q-form>

            <RouterLink to=""> </RouterLink>
          </q-card-section>
        </q-card>
      </q-page>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useQuasar } from 'quasar';
import { appApi } from 'boot/axios';
import { useAuthStore } from 'stores/auth-store';
import { useRouter, RouterLink } from 'vue-router';
//import SignaturePad from 'signature_pad';

const authStore = useAuthStore();
const $q = useQuasar();
const router = useRouter();

const loginName = ref('');
const password = ref('');
const isPwd = ref(true); // password not shown in plaintext by defa
const rememberMe = ref(false); // checkbox is uncheck by default

//var signaturePad: SignaturePad = null;

const requiredRule = (fieldName: string) => (val: string) =>
  !!val || `Please enter your ${fieldName}.`;

const togglePasswordVisibility = () => {
  isPwd.value = !isPwd.value;
};

/*
onMounted(() => {
  var wrapper = document.getElementById('signature-pad');
  var canvas = wrapper.querySelector('canvas');

  signaturePad = new SignaturePad(canvas);
});

function clear(): void {
  signaturePad.clear();
}

function getData(): void {
  var test = signaturePad.toDataURL();
  console.log(test);
}
  */

const submit = async () => {
  try {
    const response = await appApi.post('api/auth/login', {
      loginName: loginName.value,
      password: password.value,
      rememberMe: rememberMe.value,
    });
    const token = response.data.token;
    authStore.updateJwtToken(token);
    router.push('/module');
  } catch (error) {
    $q.notify({
      message: 'Username or password is incorrect. Please try again.',
      color: 'negative',
    });
  }
};

defineOptions({
  name: ' LoginPage',
});
</script>
