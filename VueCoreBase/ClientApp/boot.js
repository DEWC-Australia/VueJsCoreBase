// Import VueJs libary installed by npm and configured by webpack
import Vue from 'vue';
// Import VueRouter from ./router/index.js
import router from "./router";
// Import Store (Vuex) from ./store/index.js
import store from "./store";

// Import BootstrapVue install by npm and configured by webpack
// provide Bootstrap 4 support
// scss support is provided from install packages
import BootstrapVue from "bootstrap-vue";

import VeeValidate from "vee-validate";

// import the bootstrap and bootstrap-vue css
import "../node_modules/bootstrap/dist/css/bootstrap.min.css";
import "../node_modules/bootstrap-vue/dist/bootstrap-vue.css";

//helpers
// axios interceptor for JWT refresh tokens
import "./helpers/interceptors";

import { httpHeaders } from './variables/variables.js';

// attach BootstrapVue
Vue.use(BootstrapVue);
// attach VeeValidate
Vue.use(VeeValidate, { fieldsBagName: 'veeFields' });


// Setup authorisation upon initialisation using store and axios header
import axios from "axios";
const initialStore = localStorage.getItem("store");

if (initialStore) {
    store.commit("initialise", JSON.parse(initialStore));
    if (store.getters.isAuthenticated) {
        axios.defaults.headers.common[httpHeaders.auth] = `Bearer ${
            store.state.auth.access_token
            }`;
    }
}

// create the Vue App with the root located at ./App.vue
new Vue({
    el: '#app-root',
    router,
    store,
    render: h => h(require('./App.vue'))
});
