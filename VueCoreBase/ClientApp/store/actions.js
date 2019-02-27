/*
 * Vuex Actions Summary
 * 
 * Method: this.$store.Dispatch()
 * 
 * Actions are our interfaces to components and they are dispatched using the Dispatch Vuex method. 
 * The allow axois asynchronous calls to interact with an API and contain the logic to cordinate
 * Mutation commits to update to global state based on desired program logic.
 * 
 * These are our functions that cordinate multiple Mutations within a single operation.
 * 
 */


// import the axios ajax library for use by Vuex actions
import axios from "axios";
import { apiRoutes, httpHeaders } from '../variables/variables.js';

/**
 * 
 * Authenication Actions
 * Notes:
 * 1. These actions require the following on the server side:
 * 1.a controller action implemented for the url /api/token. 
 * token route must return a token with the following contents
 *      - JWT token
 *      - refreshToken
 *      - user_id (used in the refresh token http request)
 *      - user First Name (used in the navbar to identify user)
 *      - User Last Name (used in the navbar to identify user)
 * 1.b api/account url for registering a new user
 * 
 */

/**
 * @name Login
 * @description Vuex login action used by the login component.
 * calls Mutations to manage the global state for controlling the view of
 * login controls and modals along with setting the auth object for route access
 * returns a promise that is waited until it is resolved before 
 * performing any further work.
 * 
 * @param {any} commit name of Vuex Action
 * @param {any} payload http payload for /api/token
 * 
 * @returns {Promise} returns a promise http reponse
 */



export const login = ({ commit }, payload) => {

    return new Promise((resolve, reject) => {
        // commit loginRequest Vuex Mutation
        commit("loginRequest");

        // call axios http post
        axios
            .post(apiRoutes.authenication.tokenUrl, payload)
            .then(response => {
                // successful response from the token action is the token, refrech token, user id and name
                const auth = response.data;

                // set the bearer token for all susequent axios requests
                axios.defaults.headers.common[httpHeaders.auth] = `Bearer ${
                    auth.access_token
                    }`;

                // commit loginSuccess Mutation (hide login modal)
                commit("loginSuccess", auth);
                // commit hideAuthModal Mutation
                commit("hideAuthModal");

                // return from the promise
                resolve(response);
            })
            .catch(error => {
                // commit loginError Mutation (show login modal)
                commit("loginError");

                // remove the token from axios based on an error being detected
                delete axios.defaults.headers.common[httpHeaders.auth];

                // return from the promise
                reject(error.response);
            });
    });
};

/**
 * @name register 
 * @description Vuex register action used by the register component.
 * action commits the register success or register error Mutations to manage the global registered state
 * @param {any} commit name of Vuex Action
 * @param {any} payload http payload for /api/account
 * @returns {Promise} returns a promise http reponse
 */
export const register = ({ commit }, payload) => {
    return new Promise((resolve, reject) => {
        // commit registerRequest Mutation (show login modal)
        commit("registerRequest");

        axios
            .post(apiRoutes.account.registerUrl, payload)
            .then(response => {
                // commit registerRequest Mutation
                commit("registerSuccess");

                // return from the promise
                resolve(response);
            })
            .catch(error => {
                // commit registerError Mutation
                commit("registerError");

                // return from the promise
                reject(error.response);
            });
    });
};

/**
 * @name register 
 * @description Vuex logout action used by the logout button on the Navbar
 *  commits the logout mutation to global change the state of authorisation
 *  and deletes the auth Jwt Token from the axois authorisation header
 * @param {any} commit name of Vuex Action
 * @returns {Promise} returns a promise http reponse
 */

export const logout = ({ commit }) => {
    // commit logout Mutation
    commit("logout");

    // remove the token from axios
    delete axios.defaults.headers.common["Authorization"];

};