
const capitalizeFirstLetter = (string) => {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

export const queryUpdate = (request) => {
    const capitalizedRequest = capitalizeFirstLetter(request);
    const result = `use${capitalizedRequest}Query`;

    return result;
}

export const mutationUpdate = (request) => {
    const capitalizedRequest = capitalizeFirstLetter(request);
    const result = `use${capitalizedRequest}Mutation`;

    return result;
}