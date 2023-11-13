'use client'

export function gerarId()
{
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        let r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8);
        return v.toString(16);
    });
}

export function isEmpty(value) {
    return (
        (value == null) ||
        (value.hasOwnProperty('length') && value.length === 0) ||
        (value.constructor === Object && Object.keys(value).length === 0)
    )
}
