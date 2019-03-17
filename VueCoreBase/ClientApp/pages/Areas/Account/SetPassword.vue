<template>
    <form @submit.prevent="setSubmit" class="p-2">

        <vee-form-server-errors :formServerErrors="serverErr"></vee-form-server-errors>

        <vee-form-success :message="successMessage" :formSubmitted="registered" :formServerErrors="serverErr"></vee-form-success>

        <vee-form-input :formInput="login"></vee-form-input>

        <vee-form-input :formInput="password"></vee-form-input>

        <vee-form-input :formInput="confirmPassword"></vee-form-input>


        <b-form-group>
            <b-button variant="primary" type="submit"
                      :disabled="loading">Set Password</b-button>
        </b-form-group>

    </form>
</template>
<script>
    import VeeFormSuccess from '../../../components/FormControls/VeeFormSuccess.vue';
    import VeeFormServerErrors from '../../../components/FormControls/VeeFormServerErrors.vue';
    import VeeFormInput from '../../../components/FormControls/VeeFormInput.vue';

    import { formObject, processResponseErrors, processProperties } from "../../../mixins/veeForms.js";
    import { apiRoutes } from '../../../variables/variables.js';

    import axios from "axios";

    export default {
        name: 'set-password',
        components: {
            VeeFormSuccess,
            VeeFormServerErrors,
            VeeFormInput
        },
        data() {
            return {
                login: formObject('Email'),
                password: formObject('New Password'),
                confirmPassword: formObject('Confirm New Password'),
                successMessage: 'Password Successfully Set.',
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
                                email: this.login.value,
                                password:  this.password.value,
                                confirmPassword: this.confirmPassword.value
                            };

                            this.$store.commit("startLoading");

                            axios
                                .post(apiRoutes.account.setPasswordUrl, payload)
                                .then(response => {
                                    this.serverErr = [];
                                    this.login.value = "";
                                    this.password.value = "";
                                    this.confirmPassword.value = "";
                                    this.registered = true;
                                    this.$store.commit("finishLoading");

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