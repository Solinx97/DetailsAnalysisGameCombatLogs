import React, {useState} from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { Link } from 'react-router-dom';

import './NavMenu.css';

const NavMenu = (props) => {
  const [collapsed, setCollapsed] = useState(true);

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

  const render = () => {
    return <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">CombatAnalysis.WebApp</NavbarBrand>
            <NavbarToggler onClick={toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
              {/*<ul className="navbar-nav flex-grow">*/}
              {/*  <NavItem>*/}
              {/*    <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>*/}
              {/*  </NavItem>*/}
              {/*  <NavItem>*/}
              {/*    <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>*/}
              {/*  </NavItem>*/}
              {/*  <NavItem>*/}
              {/*    <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>*/}
              {/*  </NavItem>*/}
              {/*</ul>*/}
            </Collapse>
          </Container>
        </Navbar>
      </header>;
  }

  return render();
}

export default NavMenu;
