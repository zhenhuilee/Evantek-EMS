<template>
  <q-layout view="lHh Lpr lFf">
    <q-footer class="bg-white">
      <q-toolbar>
        <q-toolbar-title></q-toolbar-title>
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

const timeStamp = Date.now();
const year = date.formatDate(timeStamp, 'YYYY');

defineOptions({
  name: 'FirstLayout',
});
</script>
