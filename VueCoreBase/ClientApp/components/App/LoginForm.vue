﻿<template>
    <form @submit.prevent="loginSubmit" class="p-2">

        <vee-form-server-errors :formServerErrors="serverErr"></vee-form-server-errors>

        <vee-form-success :message="successMessage" :formSubmitted="registered" :formServerErrors="serverErr"></vee-form-success>

        <p>Login with your e-mail address and password.</p>

        <vee-form-input :formInput="login"></vee-form-input>
        <vee-form-input :formInput="password"></vee-form-input>

        <b-form-group>
            <b-button variant="primary" type="submit"
                      :disabled="loading">Login</b-button>

            <b-button variant="dark" @click="function(){$router.push({ path: forgotLogin }); $emit('close');}"
                      :disabled="loading">Forgot Login</b-button>

            <b-button variant="light" @click="$emit('close')"
                      :disabled="loading">Cancel</b-button>
        </b-form-group>


    </form>
</template>
<script>
    import VeeFormSuccess from '../FormControls/VeeFormSuccess.vue';
    import VeeFormServerErrors from '../FormControls/VeeFormServerErrors.vue';
    import VeeFormInput from '../FormControls/VeeFormInput.vue';
    import { formObject, processResponseErrors, processProperties } from "../../mixins/veeForms.js";
    import { clientRoutes } from '../../variables/variables.js';
    export default {
        name: "login-form",
        components: {
            VeeFormSuccess,
            VeeFormServerErrors,
            VeeFormInput
        },
        props: {
            registered: {
                type: Boolean,
                required: false
            }
        },
        data() {
            return {
                password: formObject('Password', null, 'password', { required: true, max: 100 }),
                login: formObject('Email', null, 'email', { required: true, email: true, max: 250 }),
                successMessage : 'Registration successful. Please check your email to confirm your account before attempting to login.',
                serverErr: [],
                forgotLogin: clientRoutes.forgotLogin
            };
        },
        computed: {
            loading() {
                return this.$store.state.loading;
            }
        },
        methods: {
            loginSubmit() {

                this.$validator.validateAll().then(
                    result => {
                        if (result) {
                            const payload = {
                                email: this.login.value,
                                password: this.password.value
                            };

                            this.$store
                                .dispatch("login", payload)
                                .then(response => {
                                    this.serverErr = [];
                                    this.login.value = "";
                                    this.password.value = "";
                                    if (this.$route.query.redirect) {
                                        this.$router.push(this.$route.query.redirect);
                                    }
                                })
                                .catch(error => {

                                    this.serverErr = processResponseErrors(error);

                                    processProperties(error,
                                        this._data, false, false);
  
                                });//end of catch
                        }
                    }
                );
                    
            }
            
        }

    }
</script>