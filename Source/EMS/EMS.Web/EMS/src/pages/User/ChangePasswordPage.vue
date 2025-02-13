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
            <q-card-section>
              <q-form class="q-px-sm q-pt-xl" @submit.prevent="submit">
                <h3 class="text-center shift-up">Change password</h3>
                <!-- Current Password-->
                <q-input
                  outlined
                  v-model="currentPassword"
                  :type="isCurrentPwd ? 'password' : 'text'"
                  label="Current Password"
                  lazy-rules
                  :rules="[
                    (val: string | any[]) => val && val.length > 0 || 'Please enter your current password.',
                    validateCurrentPassword
                  ]"
                 
                >
                  <template v-slot:prepend>
                    <q-icon name="lock" />
                  </template>

                  <template v-slot:append>
                    <q-icon
                      :name="isCurrentPwd ? 'visibility_off' : 'visibility'"
                      class="cursor-pointer"
                      @click="isCurrentPwd = !isCurrentPwd"
                    />
                  </template>
                </q-input>
               
                <!-- New Password -->
                <q-input
                  outlined
                  v-model="newPassword"
                  :type="isNewPwd ? 'password' : 'text'"
                  label="New Password"
                  lazy-rules
                  :rules="[ 
                    (val: string | any[]) => val && val.length > 0 || 'Please enter your new password.',
                    (val: string) => val.length >= 8 || 'Password must be at least 8 characters long.',
                    (val: string) => /[A-Z]/.test(val) || 'Password must contain at least 1 uppercase letter.',
                    (val: string) => /[a-z]/.test(val) || 'Password must contain at least 1 lowercase letter.',
                    (val: string) => /[0-9]/.test(val) || 'Password must contain at least 1 digit.',
                    (val: string) => /[^A-Za-z0-9]/.test(val) || 'Password must contain at least 1 special character.'
                  ]"
                  class="q-mt-md"
                >
                  <template v-slot:prepend>
                    <q-icon name="lock" />
                  </template>

                  <template v-slot:append>
                    <q-icon
                      :name="isNewPwd ? 'visibility_off' : 'visibility'"
                      class="cursor-pointer"
                      @click="isNewPwd = !isNewPwd"
                    />
                  </template>
                </q-input>
                
                <!-- Confirm Password -->
                <q-input
                  outlined
                  v-model="confirmPassword"
                  :type="isConfirmPwd ? 'password' : 'text'"
                  label="Re-enter Password"
                  lazy-rules
                  :rules="[ 
                    (val: string | any[]) => val && val.length > 0 || 'Please enter the password again.',
                    (val: string) => val === newPassword || 'Re-enter password must be the same as new password.'
                  ]"
                  class="q-mt-md"
                >
                  <template v-slot:prepend>
                    <q-icon name="lock" />
                  </template>

                  <template v-slot:append>
                    <q-icon
                      :name="isConfirmPwd ? 'visibility_off' : 'visibility'"
                      class="cursor-pointer"
                      @click="isConfirmPwd = !isConfirmPwd"
                    />
                  </template>
                </q-input>
                
                <q-btn
                unelevated
                size="lg"
                style="background-color: #1f459a"
                class="full-width text-white q-mt-md"
                type="Submit"
                label="change password"
              />
              

              </q-form>
            </q-card-section>
          </q-card-section>
        </q-card>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

<style>
.shift-up {
  margin-top: -10px; /* Adjust this value as needed */
}
</style>

<script setup lang="ts">
import { ref } from 'vue';
import { useQuasar } from 'quasar';
import { appApi } from 'boot/axios';

const $q = useQuasar();

const currentPassword = ref('');
const newPassword = ref('');
const confirmPassword = ref('');

const isCurrentPwd = ref(true); // visibility is off by default
const isNewPwd = ref(true);
const isConfirmPwd = ref(true);

const validateCurrentPassword = async (value: string) => {
  try {
    const response = await appApi.post('/api/ChangePassword/ValidateCurrentPassword', {
      currentPassword: value
    });
    if (response.data) {
      return true;
    } else {
      return 'Current password is incorrect.';
    }
  } catch (error) {
    return 'Validation error. Please try again.';
  }
};

const submit = async () => {
  try {
    const response = await appApi.post('/api/ChangePassword/changepassword', {
      currentPassword: currentPassword.value,
      newPassword: newPassword.value,
      confirmPassword: confirmPassword.value,
    });

    if (response.status == 200) {
      $q.notify({
        type: 'positive',
        message: 'Password changed successfully.',
      });
      // Clear the input fields
      currentPassword.value = '';
      newPassword.value = '';
      confirmPassword.value = '';
    } else {
      $q.notify({
        type: 'negative',
        message: 'An error occurred while changing the password. Please try again.',
      });
    }
  } catch (error) {
    // Handle unexpected errors
    $q.notify({
      type: 'negative',
      message: 'An unexpected error occurred. Please try again.',
    });
  }
};
</script>