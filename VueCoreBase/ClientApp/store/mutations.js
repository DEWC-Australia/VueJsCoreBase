/*
 * Vuex Mutation Summary
 * 
 * Method: this.$store.Commit()
 * 
 * Mutations are the only manner for which global state change can occur.
 * They do not contain any logic and only perform assignment on Vuex variables.
 * 
 * Mutations are called by Vuex commits and are synchronous in nature.
 * 
 * This allows for a Mutation history to be generated to ensure a history of state changes 
 * is transparent and traceable.
 * 
 * 
 */

export const initialise = (state, payload) => {
    Object.assign(state, payload);
};


export const showAuthModal = state => {
    state.showAuthModal = true;
};
export const hideAuthModal = state => {
    state.showAuthModal = false;
};

export const loginRequest = state => {
    state.loading = true;
};
export const loginSuccess = (state, payload) => {
    state.auth = payload;
    state.loading = false;
};
export const loginError = state => {
    state.loading = false;
};

export const registerRequest = state => {
    state.loading = true;
};
export const registerSuccess = state => {
    state.loading = false;
};
export const registerError = state => {
    state.loading = false;
};

/**
 * on logout set auth to null
 * @param {any} state
 */

export const logout = state => {
    state.auth = null;
};

export const startLoading = state => {
    state.loading = true;
};

export const finishLoading = state => {
    state.loading = false;
};