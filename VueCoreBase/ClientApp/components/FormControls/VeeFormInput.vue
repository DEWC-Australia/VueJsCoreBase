<template>
    <b-form-group :label="formInput.displayName">
        <b-form-input v-model.trim="formInput.value" :name="formInput.displayName" v-validate="formInput.validations" :state="state(formInput.displayName)" :type="formInput.type" />
        <b-form-invalid-feedback>
            {{ errors.first(formInput.displayName)}}
        </b-form-invalid-feedback>
    </b-form-group>
</template>
<script>
    import { fieldState } from "../../mixins/veeForms.js";

    export default {
        name: 'vee-form-input',
        props: {
            formInput: {
                type: Object,
                required: true
            }
        },
        methods: {
            state(field) {
                return fieldState(field, this.errors, this.veeFields);
            }
        }
    }
</script>