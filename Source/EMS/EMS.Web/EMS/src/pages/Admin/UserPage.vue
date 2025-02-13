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
            <h3 class="text-center shift-down">Users</h3>
            <div class="q-pa-md">
              <q-table
                flat
                bordered
                title="All users"
                :rows="rows"
                :columns="columns"
                row-key="id"
                :separator="separator"
              >
                <template v-slot:top
                  >All Users
                  <!-- Add button-->
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
                    <q-btn
                      style="background-color: #1f459a"
                      class="text-white"
                      label="..."
                    >
                      <q-menu>
                        <q-list style="min-width: 100px">
                          <!-- Edit button -->
                          <q-item
                            clickable
                            v-close-popup
                            @click="
                              openEditDialog(
                                props.row.userId,
                                props.row.userName,
                                props.row.note,
                                props.row.statusId,
                                props.row.roleId,
                                props.row.emailAddress
                              )
                            "
                          >
                            <q-item-section>Edit</q-item-section>
                          </q-item>

                          <!-- Change Password button-->
                          <q-item
                            clickable
                            v-close-popup
                            @click="openChangePasswordDialog(props.row.userId)"
                          >
                            <q-item-section>Change Password</q-item-section>
                          </q-item>

                          <!-- Delete button -->
                          <q-item
                            clickable
                            v-close-popup
                            @click="
                              openDeleteDialog(
                                props.row.userId,
                                props.row.userName
                              )
                            "
                          >
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
      <q-card style="min-width: 350px">
        <q-card-section>
          <q-form @submit.prevent="addUser">
            <div class="row justify-end">
              <q-btn icon="close" flat round dense v-close-popup />
            </div>

            <!-- Name-->
            <q-input
              outlined
              v-model="name"
              label="Name"
              lazy-rules
              :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter a name.']"
              class="q-mt-md"
            />

            <!-- Username -->
            <q-input
              outlined
              v-model="loginName"
              label="Username"
              lazy-rules
              :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter a username.']"
              class="q-mt-md"
            />

            <!-- Password -->
            <q-input
              v-model="password"
              outlined
              :type="isPwd ? 'password' : 'text'"
              label="Password"
              lazy-rules
              :rules="[ 
                (val: string | any[]) => val && val.length > 0 || 'Please enter a password.',
                (val: string) => val.length >= 8 || 'Password must be at least 8 characters long.',
                (val: string) => /[A-Z]/.test(val) || 'Password must contain at least one uppercase letter.',
                (val: string) => /[a-z]/.test(val) || 'Password must contain at least one lowercase letter.',
                (val: string) => /[0-9]/.test(val) || 'Password must contain at least one digit.',
                (val: string) => /[^A-Za-z0-9]/.test(val) || 'Password must contain at least one special character.'
              ]"
              class="q-mt-md"
            >
              <template v-slot:prepend>
                <q-icon name="lock" />
              </template>

              <template v-slot:append>
                <q-icon
                  :name="isPwd ? 'visibility_off' : 'visibility'"
                  class="cursor-pointer"
                  @click="isPwd = !isPwd"
                />
              </template>
            </q-input>

            {{ roleIds }}
            <!-- Role -->
            <q-select
              outlined
              v-model="roleIds"
              :options="options"
              label="Roles"
              multiple
              emit-value
              map-options
              lazy-rules
              :rules="[(val: string | any[]) => val && val.length > 0 || 'Please select a role.']"
              class="q-mt-md"
            >
              <template
                v-slot:option="{ itemProps, opt, selected, toggleOption }"
              >
                <q-item v-bind="itemProps">
                  <q-item-section>
                    <q-item-label>{{ opt.label }}</q-item-label>
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

            <!-- Email Address -->
            <q-input
              outlined
              v-model="emailAddress"
              label="Email Address"
              lazy-rules
              :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter an email address.']"
              class="q-mt-md"
            />

            <q-btn
              unelevated
              size="lg"
              style="background-color: #1f459a"
              class="full-width text-white q-mt-md"
              label="add"
              type="addUser"
            />
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>

    <!-- Edit pop up -->
    <q-dialog v-model="edit">
      <q-card style="min-width: 350px">
        <q-card-section>
          <q-form @submit.prevent="editUser">
            <div class="row justify-end">
              <q-btn icon="close" flat round dense v-close-popup />
            </div>

            <!-- Username -->
            <q-input
              outlined
              v-model="editName"
              label="Name"
              lazy-rules
              :rules="[(val: string | any[]) => val && val.length > 0 || 'Please enter a name.']"
              class="q-mt-md"
            />

            <!-- Status dropdown -->
            <q-select
              outlined
              v-model="editStatus"
              :options="statusOptions"
              :loading="statusLoading"
              :disable="statusLoading"
              option-value="value"
              option-label="name"
              label="Status"
              class="q-mt-md"
            />

            <br />

            <!-- Note -->
            <q-input
              outlined
              v-model="editNote"
              label="Note"
              autogrow
              lazy-rules
              :rules="[(val: string) => val.length <= 50 || 'Note cannot exceed 50 characters.']"
              class="q-mt-md"
            />

            <!-- Roles -->
            <q-select
              outlined
              v-model="editRoles"
              :options="options"
              label="Roles"
              multiple
              emit-value
              map-options
              lazy-rules
              :rules="[(val: string | any[]) => val && val.length > 0 || 'Please select a role.']"
              class="q-mt-md"
            >
              <template
                v-slot:option="{ itemProps, opt, selected, toggleOption }"
              >
                <q-item v-bind="itemProps">
                  <q-item-section>
                    <q-item-label>{{ opt.label }}</q-item-label>
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

            <!-- Email Address -->
            <q-input
              outlined
              v-model="editEmailAddress"
              label="Email Address"
              lazy-rules
              :rules="[ (val: string | any[]) => val && val.length > 0 || 'Please enter an email address.']"
              class="q-mt-md"
            />

            <q-btn
              unelevated
              size="lg"
              style="background-color: #1f459a"
              class="full-width text-white q-mt-md"
              type="editUser"
              label="edit"
            />
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>

    <!-- Change password pop up -->
    <q-dialog v-model="changePassword">
      <q-card style="min-width: 350px">
        <q-card-section>
          <q-form @submit.prevent="changePasswordBtn">
            <div class="row justify-end">
              <q-btn icon="close" flat round dense v-close-popup />
            </div>
            <br />
            <!-- New Password -->
            <q-input
              v-model="NewPassword"
              outlined
              :type="isNewPwd ? 'password' : 'text'"
              label="New password"
              lazy-rules
              :rules="[ 
              (val: string | any[]) => val && val.length > 0 || 'Please enter a new password.',
              (val: string) => val.length >= 8 || 'Password must be at least 8 characters long.',
              (val: string) => /[A-Z]/.test(val) || 'Password must contain at least one uppercase letter.',
              (val: string) => /[a-z]/.test(val) || 'Password must contain at least one lowercase letter.',
              (val: string) => /[0-9]/.test(val) || 'Password must contain at least one digit.',
              (val: string) => /[^A-Za-z0-9]/.test(val) || 'Password must contain at least one special character.'
            ]"
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
            <br />
            <q-btn
              unelevated
              size="lg"
              style="background-color: #1f459a"
              class="full-width text-white"
              type="changePasswordBtn"
              label="Change Password"
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
            <!-- User dropdown -->
            <q-form>
              <h5>
                Do you want to delete <strong>{{ selectedUserName }}</strong> ?
              </h5>
            </q-form>
            <br />
            <q-btn
              unelevated
              size="lg"
              style="background-color: #1f459a"
              class="full-width text-white"
              @click="deleteBtn(selectedUserId)"
              :label="btnLabel4"
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

const options = ref([
  { label: 'Admin', value: 1 },
  { label: 'User', value: 2 },
  { label: 'Engineer', value: 3 },
]);

const editRoles = ref([]); // v-model
const editUserId = ref(0);
const editName = ref(''); // v-model
const editNote = ref(''); // v-model
const editStatus = ref(null); // v-model
const editEmailAddress = ref(''); // v-model

const roleIds = ref([]);

const name = ref('');
const loginName = ref('');
const password = ref('');
const isPwd = ref(true); // true will hide password
const selectedStatus = ref(null);
const statusLoading = ref(true);
const statusOptions = ref([]);

const emailAddress = ref('');

const columns: QTableProps['columns'] = [
  {
    name: 'Name',
    required: true,
    label: 'Name',
    align: 'left',
    field: (row: { userName: string }) => row.userName,
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
    name: 'Note',
    align: 'left',
    label: 'Note',
    field: (row: { note: string }) => row.note,
    format: (val: string) => `${val}`,
    sortable: false,
  },
  {
    name: 'Roles',
    align: 'left',
    label: 'Roles',
    field: (row: { roleName: string }) => row.roleName,
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
const edit = ref(false);
const add = ref(false);

class Option {
  name: string;
  value: number;
  isCategory: boolean;
}

async function fetchData() {
  try {
    console.log('Fetching data...');
    const response = await appApi.get('/api/User/GetUserList');
    console.log('Data fetched:', response.data);

    // Sort rows by 'userName' in alphabetical order
    rows.value = response.data.sort((a: any, b: any) => {
      return a.userName.localeCompare(b.userName);
    });
  } catch (error) {
    console.error('Error fetching data:', error);
    $q.notify({ color: 'negative', message: 'Failed to fetch data' });
  }
}

onMounted(async () => {
  try {
    const response = await appApi.get('api/Attendance/GetStatuslist');

    statusOptions.value.length = 0;

    response.data.forEach(
      (element: {
        categoryName: string; // Office id: 2
        statuses: Array<{ id: number; name: string }>; //id: 14 Office
      }) => {
        element.statuses.forEach((status: { id: number; name: string }) => {
          var statusOption: Option = {
            name: `${element.categoryName} - ${status.name}`,
            value: status.id,
            isCategory: false,
          };
          statusOptions.value.push(statusOption);
          console.log(statusOption);
        });
      }
    );
    statusLoading.value = false;
  } catch (error) {
    console.log(error);
    $q.notify({
      message: 'Failed to retrieve status list. Please contact admin.',
      color: 'negative',
    });
  }

  try {
    const response = await appApi.get('api/Attendance/GetUserAttendance');
    const data = response.data;

    selectedStatus.value = statusOptions.value.find((element) => {
      return element.value == data.statusId;
    });
  } catch (error) {
    console.log(error);
    $q.notify({
      message: 'Failed to retrieve user attendance. Please contact admin.',
      color: 'negative',
    });
  }
});

// Add
const addUser = async () => {
  try {
    const userData = {
      name: name.value,
      loginName: loginName.value,
      password: password.value,
      roleIds: roleIds.value,
      emailAddress: emailAddress.value,
    };

    //console.log('Submitting user data:', userData);

    const response = await appApi.post('/api/User/AddUser', userData);

    if (response.status === 200) {
      $q.notify({
        color: 'positive',
        message: 'User added successfully.',
      });

      await fetchData();
      add.value = false;

      // Reset fields
      name.value = '';
      loginName.value = '';
      password.value = '';
      roleIds.value = [];
      emailAddress.value = '';
    } else {
      console.error('Failed to add user, status code:', response.status);
    }
  } catch (error) {
    console.error('Error adding user:', error);
    $q.notify({
      message: 'Failed to add user. Please try again ok.',
      color: 'negative',
    });
  }
};

onMounted(() => {
  fetchData();
});

// Edit
function openEditDialog(
  userId: number,
  username: string,
  note: string,
  selectedStatusId: number,
  roleIds: Array<number>, // role id 1 or 2
  emailAddress: string
) {
  editUserId.value = userId;
  editName.value = username;
  editNote.value = note;
  editStatus.value = statusOptions.value.find(
    (element) => element.value == selectedStatusId
  );
  editRoles.value.length = 0;
  roleIds.forEach((roleId) => {
    const role = options.value.find((element) => element.value == roleId);
    if (role) {
      editRoles.value.push(role.value);
    }
  });
  editEmailAddress.value = emailAddress;
  edit.value = true;
}

console.log(emailAddress);

const editUser = async () => {
  try {
    const updatedUserData = {
      userId: editUserId.value,
      userName: editName.value,
      note: editNote.value,
      statusId: editStatus.value.value,
      roleIds: editRoles.value,
      emailAddress: editEmailAddress.value,
    };

    console.log('Submitting edited user data:', updatedUserData);

    const response = await appApi.post(
      '/api/User/EditUserInfo',
      updatedUserData
    );

    if (response.status === 200) {
      $q.notify({
        message: "User's information updated successfully",
        color: 'positive',
      });

      edit.value = false; // Close the dialog
      await fetchData(); // Refresh the user list
    } else {
      console.error(
        'Failed to update user information, status code:',
        response.status
      );
      $q.notify({
        message: 'Failed to update user information. Please try again.',
        color: 'negative',
      });
    }
  } catch (error) {
    console.error('Error updating user information:', error);
    $q.notify({
      message: 'Failed to update user information. Please try again.',
      color: 'negative',
    });
  }
};

// Change Password
const NewPassword = ref(''); // v-model="NewPassword"
const isNewPwd = ref(true); // :type="isNewPwd true will not show plaintext password

const changePassword = ref(false); // v-model="changePassword" changePassword is close by default(false)
const selectedUserId = ref(0); // store the user id

function openChangePasswordDialog(userId: number) {
  selectedUserId.value = userId; // open the pop up for the user with that user id
  changePassword.value = true; // open the change password pop up
}
const changePasswordBtn = async () => {
  // use axios to call the api
  try {
    //console.log('Changing password for userId:', selectedUserId.value);
    const response = await appApi.post('/api/User/AdminChangePassword', {
      userId: selectedUserId.value,
      newPassword: NewPassword.value,
    });

    if (response.status === 200) {
      $q.notify({
        message: 'Password changed successfully',
        color: 'positive',
      });
      NewPassword.value = '';
      changePassword.value = false;
    } else {
      console.error('Failed to change password, status code:', response.status);
      $q.notify({
        message: 'Failed to change password. Please try again.',
        color: 'negative',
      });
    }
  } catch (error) {
    console.error('Error changing password:', error);
    $q.notify({
      message: 'Failed to change password. Please try again.',
      color: 'negative',
    });
  }
};

// Delete
const remove = ref(false);
const btnLabel4 = ref('DELETE');
const selectedUserName = ref('');

function openDeleteDialog(userId: number, userName: string) {
  selectedUserId.value = userId; // open the pop up for the user with that user id
  selectedUserName.value = userName;
  remove.value = true; // open the change password pop up
}

const deleteBtn = async (userId: number) => {
  try {
    const response = await appApi.delete(`/api/User/DeleteUser/${userId}`);
    if (response.status === 200) {
      $q.notify({
        message: 'User deleted successfully',
        color: 'positive',
      });

      await fetchData(); // Refresh the user list
      remove.value = false; // Close the dialog
    } else {
      $q.notify({
        message: 'Failed to delete user. Please try again.',
        color: 'negative',
      });
    }
  } catch (error) {
    console.error('Error deleting user:', error);
    $q.notify({
      message: 'Failed to delete user. Please try again.',
      color: 'negative',
    });
  }
};
</script>
