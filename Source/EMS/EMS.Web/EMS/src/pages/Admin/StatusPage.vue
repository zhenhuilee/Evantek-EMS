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
            <h3 class="text-center shift-down">Status</h3>
            <div class="q-pa-md">
              <q-table
                flat bordered
                :rows="rows"
                :columns="columns"
                row-key="id"
                :separator="separator"
              >
              <template v-slot:top
                  >All Statuses
                  <!-- Add button -->
                  <q-btn
                    class="q-ml-auto text-white q-pa-sm"
                    label="Add"
                    style="background-color: #1f459a"
                    dense
                    icon="add"
                    @click="add = true"
                  />
                </template>

                <template v-slot:body-cell-action="props">
                  <q-td :props="props" align="left">
                    <q-btn class="text-white" style="background-color: #1f459a" label="...">
                      <q-menu>
                        <q-list style="min-width: 100px">

                          <!-- Edit button -->
                          <q-item
                            clickable
                            v-close-popup
                            @click="
                              openEditDialog(
                                props.row.categoryId,
                                props.row.categoryName,
                                props.row.statusId,
                                props.row.statusName,
                              )
                            ">
                            <q-item-section>Edit</q-item-section>
                          </q-item>

                          <!-- Delete button -->
                          <q-item
                            clickable
                            v-close-popup
                            @click="openDeleteDialog(
                                props.row.categoryName,
                                props.row.statusId,
                                props.row.statusName,
                                )">
                            <q-item-section>Delete</q-item-section>
                          </q-item>


                        </q-list>
                      </q-menu>
                    </q-btn>
                  </q-td>
                </template>

              </q-table>
            </div>
          </q-card-section>
        </q-card>
      </q-page>
    </q-page-container>

      <!-- Add pop up -->
      <q-dialog v-model="add">
      <q-card style="min-width: 350px;">
        <q-card-section>
          <q-form @submit.prevent="addStatus">
            <div class="row justify-end">
              <q-btn icon="close" flat round dense v-close-popup />
            </div>
            <!-- Category -->
            <q-select 
            outlined 
            v-model="categoryId" 
            :options="categoryOptions" 
            label="Category" 
            emit-value
            map-options
            class="q-mt-md"
            />
            
            
            <!-- Status -->
            <q-input
              outlined
              v-model="statusName"
              label="Status"
              class="q-mt-md"
              lazy-rules
              :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter a status for the selected category.']"
            />
            
            <q-btn
              unelevated
              size="lg"
              style="background-color: #1f459a"
              class="full-width text-white q-mt-md"
              label="add"
              type="addStatus"
            />
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>

    <!-- Edit pop up -->
    <q-dialog v-model="edit">
      <q-card style="min-width: 350px">
        <q-card-section>
          <q-form @submit.prevent="editStatus">
            <div class="row justify-end">
              <q-btn icon="close" flat round dense v-close-popup />
            </div>

            <!-- Category -->
            <q-select 
            outlined 
            v-model="editCategoryId" 
            :options="categoryOptions" 
            label="Category" 
            emit-value
            map-options
            class="q-mt-md"
            />
            
            <!-- Status -->
            <q-input
             outlined 
             v-model="editStatusName"
             label="Status" 
            lazy-rules
            :rules="[(val: string | any[]) => val && val.length > 0 || 'Please enter a status.']"
             class="q-mt-md"
            />
            
            <q-btn
              unelevated
              size="lg"
              style="background-color: #1f459a"
              class="full-width text-white q-mt-md"
              type="editStatus"
              label="edit"
               
            />
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>

    <!-- Delete pop up -->
    <q-dialog v-model="remove">
      <q-card style="min-width: 350px">
        <q-card-section>
          <q-form>
            <div class="row justify-end">
              <q-btn icon="close" flat round dense v-close-popup />
            </div>
            <br />
            <q-form>
              <h5> Do you want to delete <strong>{{ selectedCategoryName }} - {{ selectedStatusName }}</strong>?</h5> 
            </q-form>
            <br />
            <q-btn
              unelevated
              size="lg"
              style="background-color: #1f459a"
              class="full-width text-white"
              @click="deleteBtn(selectedStatusId)"
              label="Delete"
              />
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>

  </q-layout>
</template>

<script setup lang="ts">
import { ref, onMounted, Ref } from 'vue';
import { QTableProps, useQuasar } from 'quasar';
import { appApi } from 'boot/axios'; 

const $q = useQuasar();

const categoryId = ref('');
const categoryOptions = ref([
  { label: 'Leave', value: 1},
  { label: 'Office', value: 2 },
  { label: 'Out Of Office', value: 3 },
  { label: 'Outstation', value: 4 },
]);

const statusName = ref('');
const editCategoryName = ref(''); 
const editStatusName = ref(''); 
const editStatusId = ref(0);
const editCategoryId = ref(0); 

const selectedStatusId = ref(0);

const columns: QTableProps['columns'] = [
  {
    name: 'Category',
    required: true,
    label: 'Category',
    align: 'left',
    field: (row: { categoryName: string }) => row.categoryName,
    format: (val: string) => `${val}`,
    sortable: false,
  },
  {
    name: 'Status',
    align: 'left',
    label: 'Status',
    field: (row: { statusName: string }) => row.statusName,
    format: (val: string) => `${val}`,
    sortable: false,
  },
  {
    name: 'action',
    align: 'center', // Center align the action column
    label: 'Action',
    field: 'action',
    sortable: false,
  },
];
const rows = ref([]);
const separator: Ref<'cell' | 'horizontal' | 'vertical' | 'none'> = ref('cell');


async function fetchData() {
  try {
    const response = await appApi.get('/api/Status/GetStatusList');
    console.log('Data fetched:', response.data);
    rows.value = response.data.sort((a: any, b: any) => {
      return a.categoryName.localeCompare(b.categoryName);
    });
    
  } catch (error) {
    console.error('Error fetching data:', error);
    $q.notify({ color: 'negative', message: 'Failed to fetch data' });
  }
}
onMounted(() => {
  fetchData();
});

// Add
const add = ref(false);
const addStatus = async () => {
  try {
    const userData = {
      categoryId: categoryId.value,
      statusName: statusName.value,
    };

    console.log('Submitting user data:', userData);

    const response = await appApi.post('/api/Status/AddStatus', userData);

    if (response.status === 200) {
      $q.notify({
        message: 'Status added successfully.',
        color: 'positive',
      });

      await fetchData();
      add.value = false;

      // Reset fields
      categoryId.value = ''; 
      statusName.value = '';
    } else {
      console.error('Failed to add status, status code:', response.status);
    }
  } catch (error) {
    console.error('Error adding status:', error);
    $q.notify({
      message: 'Failed to add status. Please try again.',
      color: 'negative',
    });
  }
};

// Edit
const edit = ref(false);

function openEditDialog(
  categoryId: number,
  categoryName: string,
  statusId: number,
  statusName: string, 
) {
  editCategoryId.value = categoryId;
  editCategoryName.value = categoryName;
  editStatusId.value = statusId;
  editStatusName.value = statusName;

  edit.value = true;
}
const editStatus = async () => {
  try {
    const updatedStatusData = {
      categoryId: editCategoryId.value,
      categoryName: editCategoryName.value,
      statusId: editStatusId.value,
      statusName: editStatusName.value
    };

    //console.log('Submitting edited data:', updatedStatusData);

    const response = await appApi.put('/api/Status/EditStatus', updatedStatusData);

    if (response.status === 200) {
      $q.notify({
        message: 'Status updated successfully.',
        color: 'positive',
      });

      edit.value = false; // Close the dialog
      await fetchData(); // Refresh the status list
    } else {
      console.error('Failed to update status information, status code:', response.status);
      $q.notify({
        message: 'Failed to update status information. Please try again.',
        color: 'negative',
      });
    }
  } catch (error) {
    console.error('Error updating status information:', error);
    $q.notify({
      message: 'Failed to update status information. Please try again.',
      color: 'negative',
    });
  }
};

// Delete
const remove = ref(false);
const selectedCategoryName = ref ('');
const selectedStatusName = ref ('');

function openDeleteDialog( categoryName : string, statusId: number, statusName: string) {
  selectedCategoryName.value =  categoryName;
  selectedStatusId.value = statusId;
  selectedStatusName.value = statusName;
  remove.value = true;  
}
const deleteBtn = async (statusId: number) => {
  try {
    const response = await appApi.delete(`/api/Status/DeleteStatus/${statusId}`);

    if (response.status === 200) {
      $q.notify({
        message: 'Status deleted successfully.',
        color: 'positive',
      });

      await fetchData(); // Refresh the user list
      remove.value = false; // Close the dialog
    } else {
      $q.notify({
        message: 'Failed to delete status. Please try again.',
        color: 'negative',
      });
    }
  } catch (error) {
    console.error('Error deleting status:', error);
    $q.notify({
      message: 'Failed to delete status. Please try again.',
      color: 'negative',
    });
  }
};

</script>