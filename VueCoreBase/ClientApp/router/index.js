// Import VueJs libary installed by npm and configured by webpack
import Vue from "vue";
// Import VueRouter libary installed by npm and configured by webpack
import VueRouter from "vue-router";
// Import NProgress libary installed by npm and configured by webpack
// Provides a progress spinner to indicate background web activity
import NProgress from "nprogress";

// import the route definitions
import routes from "./routes";
// import the the Vuex Store
import store from "../store";

Vue.use(VueRouter);

// import the definde routes into the router
const router = new VueRouter({ mode: "history", routes: routes });

// define the functionality to occur before every client-side router redirect
router.beforeEach((to, from, next) => {
    // start the activity indicator
    NProgress.start();

    // 
    if (to.matched.some(route =>
        route.meta.requiresAuth)) {

        if (!store.getters.isAuthenticated) {

            store.commit("showAuthModal");
            next({ path: from.path, query: { redirect: to.path } });

        } else {

            if (to.matched.some(
                route => route.meta.role && store.getters.isInRole(route.meta.role)
            )) {
                next();
            } else if (!to.matched.some(
                route => route.meta.role)) {
                next();
            } else {
                next({ path: "/" });
            }
        }
    } else {
        if (to.matched.some(
            route =>
                route.meta.role &&
                (!store.getters.isAuthenticated ||
                    store.getters.isInRole(route.meta.role))
        )) {
            next();
        } else {
            if (to.matched.some(route => route.meta.role)) {
                next({ path: "/" });
            }

            next();
        }
    }
});

// after the redirect
router.afterEach((to, from) => {
    // stop the activity indicator
    NProgress.done();
});

export default router;
