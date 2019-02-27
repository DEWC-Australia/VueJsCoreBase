// add http ajax support
import axios from "axios";
// add access to the Vuex Store
import store from "../store";
// add access to the client-side router
import router from "../router";
import { apiRoutes, httpHeaders } from '../variables/variables.js';

/*
 * @name use
 * @decription
 * 
 * @params 
 
 * */

/**
 * @name use
 * @description checks that a http response is not for an invalid JWT Token to trigger a refresh token retry
 * 
 * first callback function of Use is a success callback
 *  - Not Required as this is checking for invalid refresh tokens
 *
 * second callback function of Use is for unsuccessful http requests
 * 
 * @param {function} success http response success callback 
 * @param {function} error http reponse error callback
 * @returns {Promise} returns a promise
 */

axios.interceptors.response.use(undefined, function (error) {
    // get the orignal request object from the error
    const originalRequest = error.config;
    // check that the error was a bad request
    if (error.response.status === 401 &&
        // request wasn't a retry
        !originalRequest._retry &&
        // that we have a valid refresh token
        store.state.auth.refresh_token) {

        // setup the retry request to refresh the token
        originalRequest._retry = true;

        const payload = {
            refresh_token: store.state.auth.refresh_token,
            user_id: store.state.auth.user_id
        };

        return axios
            .post(apiRoutes.authenication.refreshUrl, payload)
            .then(response => {
                const auth = response.data;

                axios.defaults.headers.common[httpHeaders.auth] = `Bearer ${
                    auth.access_token
                    }`;
                originalRequest.headers[httpHeaders.auth] = `Bearer ${
                    auth.access_token
                    }`;
                store.commit("loginSuccess", auth);

                return axios(originalRequest);
            })
            .catch(error => {
                store.commit("logout");
                router.push({ path: "/" });
                delete axios.defaults.headers.common[httpHeaders.auth];
                return Promise.reject(error);
            });
    }

    return Promise.reject(error);
});
