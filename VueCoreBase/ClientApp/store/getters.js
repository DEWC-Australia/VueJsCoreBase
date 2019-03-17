/*
 * Vuex Getters Summary
 * 
 * Method: this.$store.state..GetterName
 *
 * Getters are similar to computed properties for Vuex state, and can be used at times when we need
 * to derive state based on the state of one or multiple variables within the store (can iterate, perform math ...). 
 * 
 * Can data bind onto Getters, and it will behave exactly like a standard component-level computed property, whereby the UI will update every time
 * Vue detects a change to the computed value of the function.
 * 
 */ 
/**
 * @name isAuthenticated
 * @description Checks if the user is authenicated by having a valid token
 * @param {any} state current Vuex State
 * @returns {bool} returns true is the user is authicated and false otherwise
 */
export const isAuthenticated = state => {
    return (
        state.auth !== null &&
        state.auth.access_token !== null
    );
};

export const isInRole = (state, getters) => role => {
    const result = getters.isAuthenticated && state.auth.roles.indexOf(role) > -1;
    return result;
};

