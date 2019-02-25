<template>
    <div class="app">
        <p-navbar></p-navbar>
        <transition name="fade" mode="in-out">
            <router-view />
        </transition>
        <p-footer></p-footer>
    </div>
</template>
<script>
    import PNavbar from './Layout/Navbar.vue';
    import PFooter from './Layout/Footer.vue';
    export default {
        name: 'app',
        components: {
            PNavbar,
            PFooter
        }
    }
</script>
<style lang="scss">
    /*Force html and body to the full size of the screen and div app and page classes to 100% of the body*/
    html,
    body{
        height: 100vh;
    }

    div.app,
    div.page{
        height: 100% !important;
    }

    .fade-enter-active,
    .fade-leave-active{
        transition: opacity 0.3s ease-in-out;
    }

    .fade-enter,
    .fade-leave-to{
        opacity: 0;
    }
</style>