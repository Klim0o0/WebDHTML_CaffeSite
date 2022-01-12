import React, {useEffect, useState} from "react";
import {GetJson} from "../Utils/Utils";
import './Menu.css'
import ModalMenu from "../Forms/ModalMenu";

export default function Menu() {
    const [items, setItems] = useState([])
    const [loaded, setLoaded] = useState(false)
    const [error, setError] = useState(null)
    const [active, setActive] = useState(false)
    const [modalContent, setModalContent] = useState("")


    useEffect(() => GetJson("/api/menu", setItems, setLoaded, setError), [])


    if (loaded && !error) {
        return (<div>{items.map(x => (
            <div className="Menu">
                <div className="mob"><img src={x.imgUrl} width="320" height="180"/>
                    <div className="FoodName">{x.name}</div>
                    <div className="Description">{x.description}</div>
                    <div><span className="Price">{x.price}</span><span className="Currency">  руб.</span></div>
                </div>
                <div className="desk"><img src={x.imgUrl} width="230" height="128" onClick={() => {
                    setModalContent(<div><img src={x.imgUrl} width="600" height="400"/>
                        <div className="FoodName">{x.name}</div>
                        <div className="Description">{x.description}</div>
                        <div><span className="Price">{x.price}</span><span className="Currency">  руб.</span></div>
                    </div>)
                    setActive(true)
                }}/>
                    <div className="FoodName">{x.name}</div>
                    <div><span className="Price">{x.price}</span><span className="Currency">  руб.</span></div>
                </div>
            </div>
        ))}{<ModalMenu active={active} setActive={setActive}>{modalContent}</ModalMenu>}</div>)
    }
    if (!loaded) {
        return (<h1>Loading</h1>)
    }
    return (<h1>ERROR!!</h1>)
}