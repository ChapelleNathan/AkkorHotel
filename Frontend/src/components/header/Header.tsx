import { NavLink } from "react-router";

export default function Header() {
    return (
        <div className="d-flex">
            <NavLink className="mx-2" to={'/'}>Akkor Hotel</NavLink>
            <NavLink className="mx-2" to={'register'}>Inscription</NavLink>
            <NavLink className="mx-2" to={'login'}>Connexion</NavLink>
        </div>
    )
}