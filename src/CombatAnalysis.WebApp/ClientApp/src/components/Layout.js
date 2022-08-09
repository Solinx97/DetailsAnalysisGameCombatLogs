import React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

const Layout = (props) => {
  const render = () => {
    return <div>
        <NavMenu />
        <Container>
          {props.children}
        </Container>
      </div>;
  }

  return render();
}

export default Layout;