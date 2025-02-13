import { defineStore } from 'pinia';

export const useAuthStore = defineStore({
  id: 'auth',
  state: () => ({
    // initialize state from local storage to enable user to stay logged in
    jwtToken: localStorage.getItem('jwtToken'),
  }),
  actions: {
    updateJwtToken(token: string) {
      // update pinia state
      this.jwtToken = token; // Assuming your API responds with a token

      // store user details and jwt in local storage to keep user logged in between page refreshes
      localStorage.setItem('jwtToken', token ?? '');
    },

    removeJwtToken(){
      // remove the JWT token from Pinia state
      this.jwtToken = null; 
  
      // remove the JWT token from local storage
      localStorage.removeItem('jwtToken');
    }
  },
});