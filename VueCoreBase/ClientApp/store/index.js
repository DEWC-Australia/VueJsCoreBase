// Import VueJs libary installed by npm and configured by webpack
import Vue from "vue";
// Import Vuex global state management libary installed by npm and configured by webpack
import Vuex from "vuex";

Vue.use(Vuex);


// import all defined Vuex actions, mutation and getters for the application
import * as actions from "./actions";
import * as mutations from "./mutations";
import * as getters from "./getters";

// define the Vuex state object and the object's initial state
const state = {
    auth: null,
    showAuthModal: false,
    loading: false,
    cart: []
};

// create the Vuex store for the application by adding the define actions, getters, mutations and state object
const store = new Vuex.Store({
    strict: true,
    actions,
    mutations,
    getters,
    state: state
});

// export the Vuex store for global use
export default store;