<template>
  <q-layout view="lHh Lpr lFf">
    <q-header>
      <q-toolbar style="background-color: #1f459a">
        <q-btn icon="change_circle" @click="goToModulePage" />

        <img
          src="/images/evanteklogo2.jpg"
          style="height: 55px; width: 190px; position: relative"
        />

        <q-toolbar-title></q-toolbar-title>

        <q-btn-dropdown style="width: 195px; position: relative; left: -28px">
          <template v-slot:label>
            <q-avatar>
              <q-icon name="account_circle" />
            </q-avatar>
            <div>{{ userName }}</div>
          </template>

          <q-list>
            <q-item clickable v-close-popup @click="changePassword">
              <q-item-section>
                <q-item-label>
                  <q-avatar> <q-icon name="lock" /> </q-avatar>
                  Change Password
                </q-item-label>
              </q-item-section>
            </q-item>
          </q-list>

          <q-list>
            <q-item clickable v-close-popup @click="logOut">
              <q-item-section>
                <q-item-label>
                  <q-avatar> <q-icon name="logout" /> </q-avatar>
                  Log Out
                </q-item-label>
              </q-item-section>
            </q-item>
          </q-list>
        </q-btn-dropdown>
      </q-toolbar>
    </q-header>

    <q-footer class="bg-white">
      <q-toolbar>
        <q-toolbar-title></q-toolbar-title>
        <!-- Without this will shift the 2024 Evantek to the bottom left corner -->
        <div class="text-dark">©{{ year }} Evantek</div>
      </q-toolbar>
    </q-footer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { date } from 'quasar';
import { ref, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import { useRouter } from 'vue-router';
import { appApi } from 'boot/axios';
import { useAuthStore } from 'stores/auth-store';

const authStore = useAuthStore();
const $q = useQuasar();
const router = useRouter(); // Import the router instance
const timeStamp = Date.now();
const year = date.formatDate(timeStamp, 'YYYY');
const userName = ref(''); // Reactive variable for storing user's name

defineOptions({
  name: 'SecondLayout',
});

// Fetch the logged-in user's name
const fetchUserName = async () => {
  try {
    const response = await appApi.get('api/User/GetName');
    console.log('User Name Response:', response.data); // Debug log
    userName.value = response.data;
  } catch (error) {
    console.log(error);
    $q.notify({
      message: 'Failed to retrieve user information. Please contact admin.',
      color: 'negative',
    });
  }
};

const goToModulePage = () => {
  router.push('/module');
};

const changePassword = () => {
  router.push('/user/changepassword');
};

const logOut = () => {
  authStore.removeJwtToken();
  router.push('/');
};

onMounted(() => {
  fetchUserName();
});
</script>

<style scoped>
.q-footer .q-toolbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
</style>
