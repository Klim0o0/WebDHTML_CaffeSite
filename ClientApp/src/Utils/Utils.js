import React, {useEffect} from "react";
import {queryHelpers} from "@testing-library/react";

export function Get(url, setItems, setLoaded) {
    fetch(url)
        .then(res => res.ok)
        .then((result) => {
            setItems(result);
            setLoaded(true);
        }, (error) => {
        })

}

export function UrlWithQuery(url, query) {

    return url + queryHelpers.buildQueries(query);
}


export function GetJson(url, setItems, setLoaded, setError) {
    fetch(url)
        .then(res => res.json())
        .then((result) => {
            setItems(result);
            setLoaded(true);
        }, (error) => {
            setError(error);
            setLoaded(true);
        })
}

export function Post(url, setItems, setIsLoaded, setError, body) {
    fetch(url, {
        method: 'POST', headers: {'Content-Type': 'application/json'}, body: body
    }).then(r => Promise.all([r.ok, r.text()]))
        .then((result) => {
            setItems(result);
            setIsLoaded(true);
        }, (error) => {
            setError(error);
            setIsLoaded(true);
        })
}

export function PostJsonResponse(url, setItems, setIsLoaded, setError, body) {
    fetch(url, {
        method: 'POST', headers: {'Content-Type': 'application/json'}, body: body
    }).then(r => Promise.all([r.ok, r.json()]))
        .then((result) => {
            setItems(result);
            setIsLoaded(true);
        }, (error) => {
            setError(error);
            setIsLoaded(true);
        })
}


function TryGetJson(r) {
    let x = null
    try {
        x = r.json()
    } catch (error) {
        x = r.text();
    }
    return x
}


export function EmptyFunction() {
}