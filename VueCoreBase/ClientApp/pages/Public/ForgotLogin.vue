<template>
    <div class="container">
        <form @submit.prevent="forgotLoginSubmit" class="p-2">

            <vee-form-server-errors :formServerErrors="serverErr"></vee-form-server-errors>

            <vee-form-success :message="successMessage" :formSubmitted="registered" :formServerErrors="serverErr"></vee-form-success>

            <p>A password reset link will be emailed to your registered email address.</p>

            <vee-form-input :formInput="login"></vee-form-input>

            <b-form-group>
                <b-button variant="primary" type="submit"
                          :disabled="loading">Reset Password</b-button>
            </b-form-group>

        </form>
    </div>
</template>
<script>
    import VeeFormSuccess from '../../components/FormControls/VeeFormSuccess.vue';
    import VeeFormServerErrors from '../../components/FormControls/VeeFormServerErrors.vue';
    import VeeFormInput from '../../components/FormControls/VeeFormInput.vue';

    import { formObject, processResponseErrors, processProperties } from "../../mixins/veeForms.js";
    import { apiRoutes } from '../../variables/variables.js';

    import axios from "axios";
    
    export default {
        name: 'forgot-login',
        components: {
            VeeFormSuccess,
            VeeFormServerErrors,
            VeeFormInput
        },
        data() {
            return {
                login: formObject('Email'),
                successMessage: 'Forgotten account unlock instructions sent to your registered email address.',
                serverErr: [],
                registered: false
            };
        },
        computed: {
            loading() {
                return this.$store.state.loading;
            }
        },
        methods: {
            forgotLoginSubmit() {

                this.$validator.validateAll().then(
                    result => {
                        if (result) {

                            const payload = {
                                email: this.login.value
                            };

                            this.$store.commit("startLoading");

                            axios
                                .post(apiRoutes.account.forgotEmailUrl, payload)
                                .then(response => {
                                    this.serverErr = [];
                                    this.login.value = "";
                                    this.registered = true;
                                    this.$store.commit("finishLoading");
                                    // redirect to home
                                    //this.$router.push(clientRoutes.home);

                                })// end then
                                .catch(error => {
                                    this.serverErr = processResponseErrors(error);

                                    processProperties(error,
                                        this._data, false, false);

                                    this.$store.commit("finishLoading");
                                });// end catch

                        }// if result
                    }
                ); // end validator

            } // end forgotLoginSubmit
        } // end methods
    }
</script>