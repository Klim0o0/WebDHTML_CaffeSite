import "./ModalMenu.css"


export default function ModalMenu({active, setActive, children}) {
    return (<div className={active ? "Modal active" : "Modal"} onClick={() => setActive(false)}>
        <div className="ModalContent" onClick={e => e.stopPropagation()}>
            {children}
        </div>
    </div>)
}