import axios from "axios";
import store from "../store";
import router from "../router";

/*
 * first callback function of Use is a success callback 
 *  - Not Required as this is checking for invalid refresh tokens
 * 
 * second callback function of Use is for unsuccessful http requests
 * */
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
            .post("/api/token/refresh", payload)
            .then(response => {
                const auth = response.data;

                axios.defaults.headers.common["Authorization"] = `Bearer ${
                    auth.access_token
                    }`;
                originalRequest.headers["Authorization"] = `Bearer ${
                    auth.access_token
                    }`;
                store.commit("loginSuccess", auth);

                return axios(originalRequest);
            })
            .catch(error => {
                store.commit("logout");
                router.push({ path: "/" });
                delete axios.defaults.headers.common["Authorization"];
                return Promise.reject(error);
            });
    }

    return Promise.reject(error);
});
