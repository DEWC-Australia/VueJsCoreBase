<template>
    <form @submit.prevent="submit" class="p-2">
        <vee-form-server-errors :formServerErrors="serverErr"></vee-form-server-errors>

        <vee-form-input :formInput="firstName"></vee-form-input>
        <vee-form-input :formInput="lastName"></vee-form-input>
        <vee-form-input :formInput="email"></vee-form-input>

        <b-form-group :label="password.displayName">
            <b-form-input v-model.trim="password.value" :name="password.displayName" v-validate="password.validations" :state="state(password.displayName)" :type="password.type" ref="password" />
            <b-form-invalid-feedback>
                {{ errors.first(password.displayName)}}
            </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group :label="confirmPassword.displayName">
            <b-form-input v-model.trim="confirmPassword.value" :name="confirmPassword.displayName" v-validate="confirmPassword.validations" :state="state(confirmPassword.displayName)" :type="confirmPassword.type" data-vv-as="password" />
            <b-form-invalid-feedback>
                {{ errors.first(confirmPassword.displayName)}}
            </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group>
            <b-button variant="primary" type="submit"
                      :disabled="loading">Register</b-button>
            <b-button variant="light" @click="$emit('close')"
                      :disabled="loading">Cancel</b-button>
        </b-form-group>
    </form>
</template>
<script>

    import VeeFormServerErrors from '../FormControls/VeeFormServerErrors.vue';
    import VeeFormInput from '../FormControls/VeeFormInput.vue';
    import { formObject, processResponseErrors, processProperties, fieldState } from "../../mixins/veeForms.js";
    export default {
        name: "register-form",
        components: {
            VeeFormServerErrors,
            VeeFormInput
        },
        data() {
            return {
                firstName: formObject('First Name', null, 'text', {required: true, max: 250}),
                lastName: formObject('Last Name', null, 'text', { required: true, max: 250 }),
                email: formObject('Email', null, 'email', { required: true, email: true, max: 250 }),
                password: formObject('Password', null, 'password', { required: true, max: 100 }),
                confirmPassword: formObject('Confirm Password', null, 'password', { required: true, confirmed: 'password' }),
                serverErr: []
            };
        },
        computed: {
            loading() {
                return this.$store.state.loading;
            }
        },
        methods: {
            submit() {


                this.$validator.validateAll().then(
                    result => {
                        if (result) {

                            const payload = {
                                firstName: this.firstName.value,
                                lastName: this.lastName.value,
                                email: this.email.value,
                                password: this.password.value,
                                confirmPassword: this.confirmPassword.value
                            };

                            this.$store
                                .dispatch("register", payload)
                                .then(response => {
                                    this.serverErrors = [];
                                    this.firstName.value = "";
                                    this.lastName.value = "";
                                    this.email.value = "";
                                    this.password.value = "";
                                    this.confirmPassword.value = "";
                                    this.$emit("success");
                                    this.serverErr = [];

                                }) // End Dispatch Then
                                .catch(error => {

                                    this.serverErr = processResponseErrors(error);

                                    processProperties(error,
                                        this._data, false, false);

                                }); // end Catch

                        }// end of if Result
                    });// end of Then
                
                
                
            },
            state(field) {
                return fieldState(field, this.errors, this.veeFields);
            }

        }
    };
</script>