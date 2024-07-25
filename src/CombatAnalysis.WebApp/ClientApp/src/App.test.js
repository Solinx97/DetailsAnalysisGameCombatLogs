import React from 'react';
import { render } from '@testing-library/react';
import App from './App';
import configureMockStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import { MemoryRouter } from 'react-router-dom';

const mockStore = configureMockStore();

const initialState = {
    user: {
        value: {
            id: "96c74d4b-2004-474c-87b0-644979ffa2d4",
            username: "Solinx",
            firstName: "Oleg",
            secondName: "Fedosov",
            phoneNUmber: 111234,
            birthday: "7/21/2024 12:00:00 AM +02:00",
            aboutMe: "",
            gender: 0,
            identityUserId: "0b207685-b4fe-425a-888a-cfbf07bb8ece"
        },
    },
    customer: {
        value: null,
    },
};

const store = mockStore(initialState);

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: jest.fn(),
}));

describe('App Component', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    test('renders without crashing', () => {
        render(
            <Provider store={store}>
                <MemoryRouter>
                    <App />
                </MemoryRouter>
            </Provider>
        );
    });

    test.skip('renders the voice chat minimazed component when useMinimaze is true', () => {
        const { getByTestId } = render(<App />);
        const voiceChatMinimazed = getByTestId('voice-chat-minimazed');
        expect(voiceChatMinimazed).toBeInTheDocument();
    });

    test.skip('does not render the voice chat minimazed component when useMinimaze is false', () => {
        const { queryByTestId } = render(<App />);
        const voiceChatMinimazed = queryByTestId('voice-chat-minimazed');
        expect(voiceChatMinimazed).toBeNull();
    });

    test.skip('renders the toast container', () => {
        const { getByTestId } = render(<App />);
        const toastContainer = getByTestId('toast-container');
        expect(toastContainer).toBeInTheDocument();
    });
});