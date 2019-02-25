<template>
    <div class="app">
        <p-navbar></p-navbar>
        <transition name="fade" mode="in-out">
            <router-view />
            <auth-nav-item />
        </transition>
        <p-footer></p-footer>
        <auth-modal :show="showAuthModal" />
    </div>
</template>
<script>
    import PNavbar from './Layout/Navbar.vue';
    import PFooter from './Layout/Footer.vue';
    import AuthNavItem from "./App/AuthNavItem.vue";
    import AuthModal from "./App/AuthModal.vue";
    export default {
        name: 'app',
        components: {
            PNavbar,
            PFooter,
            AuthNavItem,
            AuthModal,
        },
        computed: {
            showAuthModal() {
                return this.$store.state.showAuthModal;
            }
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