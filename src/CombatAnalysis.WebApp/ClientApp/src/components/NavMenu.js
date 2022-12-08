import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { checkAuth } from '../features/AuthenticationReducer';
import { userUpdate } from '../features/UserReducer';

import '../styles/navMenu.css';

const NavMenu = () => {
    const isAuth = useSelector((state) => state.authentication.value);
    const user = useSelector((state) => state.user.value);
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const [collapsed, setCollapsed] = useState(true);

    useEffect(() => {
        if (user !== null) {
            return;
        }

        const checkAuthResult = async () => {
            await checkAuthAsync();
        }

        checkAuthResult().catch(console.error);
    });

    const checkAuthAsync = async () => {
        const response = await fetch('api/v1/Authentication');

        const result = await response;
        if (result.status === 200) {
            const jsonData = await result.json();

            dispatch(userUpdate(jsonData));
            dispatch(checkAuth(true));
        }
        else {
            dispatch(checkAuth(false));
        }
    }

    const logoutAsync = async () => {
        const response = await fetch('api/v1/Account/logout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.status === 200) {
            dispatch(checkAuth(false))
            dispatch(userUpdate(null));
        }
    }

    const toggleNavbar = () => {
        setCollapsed(!collapsed);
    }

    const render = () => {
        return (<header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">Wow Analysis</NavbarBrand>
                    <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar/>
                    {!isAuth
                        ? <div>
                            <button type="button" className="btn btn-primary" onClick={() => navigate('/registration')}>Registration</button>
                            <button type="button" className="btn btn-primary" onClick={() => navigate('/login')}>Login</button>
                        </div>
                        : <div>
                            <div>Welcome, <strong>{user.email}</strong></div>
                            <button type="button" className="btn btn-primary" onClick={logoutAsync}>Logout</button>
                          </div>
                    }
                </Container>
            </Navbar>
        </header>);
    }

  return render();
}

export default NavMenu;
