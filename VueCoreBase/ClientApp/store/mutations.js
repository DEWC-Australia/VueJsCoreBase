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