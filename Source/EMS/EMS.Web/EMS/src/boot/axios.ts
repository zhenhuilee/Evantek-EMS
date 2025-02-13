import { boot } from 'quasar/wrappers';
import axios, { AxiosInstance } from 'axios';

declare module '@vue/runtime-core' {
  interface ComponentCustomProperties {
    $axios: AxiosInstance;
    $api: AxiosInstance;
  }
}
// Be careful when using SSR for cross-request state pollution
// due to creating a Singleton instance here;
// If any client changes this (global) instance, it might be a
// good idea to move this instance creation inside of the
// "export default () => {}" function below (which runs individually
// for each client)

const apiBaseURL = process.env.PROD
  ? 'https://evantekemsapi.takeq.online'
  : 'https://localhost:7160';

const appApi = axios.create({ baseURL: apiBaseURL });

export default boot(({ app, router }) => {
// Attach axios and appApi instances to Vue's global properties
  app.config.globalProperties.$axios = axios;
  app.config.globalProperties.$api = appApi;


    // Request interceptor for adding Authorization header
  appApi.interceptors.request.use(config => {
  const token = localStorage.getItem('jwtToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });

     // Response interceptor for handling errors
  appApi.interceptors.response.use(
  response => response,
    error => {
      if (error.response) {
        const { status } = error.response;
        if (status === 401) { // 401 means unauthorized
          router.push('/');
        } else if (status === 403) { // 403 means forbidden
          router.push('/module');
        }
      }
      return Promise.reject(error);
    }
  );
});

export { appApi };