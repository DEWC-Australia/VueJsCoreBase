import Vue from 'vue';
import VueRouter from 'vue-router';
import BootstrapVue from "bootstrap-vue";
import NProgress from "nprogress";
import "../node_modules/bootstrap-vue/node_modules/bootstrap/dist/css/bootstrap.min.css";
import "../node_modules/bootstrap-vue/dist/bootstrap-vue.css";
import store from "./store";

Vue.use(VueRouter);
Vue.use(BootstrapVue);

import Home from "./pages/Public/Home.vue";
import Contact from './pages/Public/Contact.vue';
import About from './pages/Public/About.vue';
import Login from './pages/Public/Login.vue';
import Register from './pages/Public/Register.vue';


const routes = [
    { path: "/", component: Home },
    { path: "/contact", component: Contact},
    { path: "/about", component: About},   
    { path: "/login", component: Login },
    { path: "/register", component: Register },   
    { path: "*", redirect: "/" }
];

const router = new VueRouter({ mode: 'history', routes: routes });

router.beforeEach((to, from, next) => {
    NProgress.start();
    next();
});

router.afterEach((to, from) => {
    NProgress.done();
});

new Vue({
    el: '#app-root',
    router: router,
    store,
    render: h => h(require('./App.vue'))
});
