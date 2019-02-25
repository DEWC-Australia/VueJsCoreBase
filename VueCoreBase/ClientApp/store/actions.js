/**
 * Login action 
 * returns a promise that is waited until it is resolved before 
 * performing any further work.
 * 
 * @param {any} commit
 * @param {any} payload
 */
export const login = ({ commit }, payload) => {
    return new Promise((resolve, reject) => {
        // commit loginRequest Mutation
        commit("loginRequest");

        axios
            .post("/api/token", payload)
            .then(response => {
                const auth = response.data;
                // set the bearer token for all susequent axios requests
                // Note: this token will become invalid if it is out of date
                axios.defaults.headers.common["Authorization"] = `Bearer ${
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
                delete axios.defaults.headers.common["Authorization"];

                // return from the promise
                reject(error.response);
            });
    });
};

/**
 * register action
 * @param {any} commit
 * @param {any} payload
 */
export const register = ({ commit }, payload) => {
    return new Promise((resolve, reject) => {
        // commit registerRequest Mutation (show login modal)
        commit("registerRequest");
        axios
            .post("/api/account", payload)
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

export const logout = ({ commit }) => {
    // commit logout Mutation
    commit("logout");

    // remove the token from axios
    delete axios.defaults.headers.common["Authorization"];

};