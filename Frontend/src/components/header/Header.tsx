import { NavLink, useNavigate } from "react-router";

export default function Header() {
    const navigate = useNavigate();

    const displayLogRoutes = () => {
        if(localStorage.getItem('bearer') == null) {
            return (
                <>
                    <NavLink className="mx-2" to={'register'}>Inscription</NavLink>
                    <NavLink className="mx-2" to={'login'}>Connexion</NavLink>
                </>
            )
        } else {
            return (
                <a className="link" onClick={logout}>Déconnexion</a>
            )
        }
    }

    const logout = () => {
        localStorage.clear();
        navigate('/login')
    }

    return (
        <div className="d-flex">
            <NavLink className="mx-2" to={'/'}>Akkor Hotel</NavLink>
            {displayLogRoutes()}
        </div>
    )
}