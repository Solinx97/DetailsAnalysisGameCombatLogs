import { Container } from 'reactstrap';
import { LayoutProps } from '../types/components/LayoutProps';
import NavMenu from './NavMenu';

const Layout: React.FC<LayoutProps> = ({ children }) => {
    const render = () => {
        return (
            <div>
                <NavMenu />
                <Container tag="main">
                  {children}
                </Container>
            </div>
        );
    }

    return render();
}

export default Layout;