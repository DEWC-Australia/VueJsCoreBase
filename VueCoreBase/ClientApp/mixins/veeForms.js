﻿export function formObject(displayName, value, type, validations){

    if(type === null || type === undefined){
        type = 'text';
    }

    if (value === null || value === undefined) {
        value = '';
    }

    if (displayName === null || displayName === undefined) {
        displayName = 'Display Name not set';
    }

    if (validations === undefined) {
        validations = null;
    }


    return {
        displayName: displayName,
        type: type,
        value: value,
        validations: validations
    };
};

export function fieldState(field, errors, fields) {

    if (errors.has(field)) {
        false;
    } else {
        return fields[field] && fields[field].dirty ? true : null;
    }
};

export function processResponseErrors(errors) {

    if (errors !== undefined) {
        return errors;
    }

    return [];
};

export function processProperties(properties, data, setValue, setDisplay) {
    // test that the server has returned view model properties
    if (!(properties === null ||
        properties === undefined)) {

        // get the keys for the servers viewmodel properties
        var keys = Object.keys(properties);

        for (var i = 0; i < keys.length; i++) {
            var key = keys[i];
            if (!(data[key] === null || data[key] === undefined)) {

                if (data[key].validations !== undefined) {
                    data[key].validations = properties[key].validations;
                }// end of validations assignment

                if (data[key].value !== undefined && setValue) {
                    data[key].value = properties[key].value;
                }// end of value assignment

                if (data[key].displayName !== undefined && setDisplay) {
                    data[key].displayName = properties[key].displayName;
                }// end of displayName assignment

            }// end of property assignment

        }// end of iterating through the servers viewmodel properties

    }// end of properties null check

};