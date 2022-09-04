import React, {useState} from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { Link, useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux'

import './NavMenu.css';

const NavMenu = (props) => {
  const user = useSelector((state) => state.user.value)
  const navigate = useNavigate();

  const [collapsed, setCollapsed] = useState(true);


  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

  const render = () => {
    return <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">Wow Analysis</NavbarBrand>
            <NavbarToggler onClick={toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar/>
                {!user &&
                    <div>
                        <button type="button" className="btn btn-primary" onClick={() => navigate('/registration')}>Registration</button>
                        <button type="button" className="btn btn-primary" onClick={() => navigate('/authorizations')}>Authorization</button>
                    </div>
                }
          </Container>
        </Navbar>
      </header>;
  }

  return render();
}

export default NavMenu;
