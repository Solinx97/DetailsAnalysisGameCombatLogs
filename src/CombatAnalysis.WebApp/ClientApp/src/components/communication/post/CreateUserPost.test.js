import '@testing-library/jest-dom';
import { fireEvent, render, waitFor } from '@testing-library/react';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useCreatePostAsyncMutation } from '../../store/api/communication/Post.api';
import CreatePost from './CreatePost';

jest.mock('../../store/api/communication/Post.api', () => ({
    useCreatePostAsyncMutation: jest.fn(),
}));

jest.mock('react-i18next', () => ({
    useTranslation: () => ({
        t: (key) => key,  // Return the key as the translation
        i18n: {
            changeLanguage: jest.fn(),
        },
    }),
    initReactI18next: {
        type: '3rdParty',
        init: jest.fn(),
    },
}));

describe('CreateUserPost Component', () => {
    const user = {
        id: '123',
    };
    const owner = '456';
    const postTypeName = 'user';
    const createTypeOfPostFunc = jest.fn();
    const { t } = useTranslation();

    beforeEach(() => {
        useCreatePostAsyncMutation.mockReturnValue([
            jest.fn().mockResolvedValue({ data: { id: '789' } }),
        ]);
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('renders loading component when user is null', () => {
        const { getByText } = render(
            <CreatePost
                user={null}
                owner={owner}
                postTypeName={postTypeName}
                createTypeOfPostFunc={createTypeOfPostFunc}
                t={t}
            />
        );

        const loadingComponent = getByText('Loading');
        expect(loadingComponent).toBeInTheDocument();
    });

    test('renders create post button', () => {
        const { getByTitle } = render(
            <CreatePost
                user={user}
                owner={owner}
                postTypeName={postTypeName}
                createTypeOfPostFunc={createTypeOfPostFunc}
                t={t}
            />
        );

        const createPostButton = getByTitle('NewPost');
        expect(createPostButton).toBeInTheDocument();
    });

    test('toggles create post form when create post button is clicked', () => {
        const { getByTitle } = render(
            <CreatePost
                user={user}
                owner={owner}
                postTypeName={postTypeName}
                createTypeOfPostFunc={createTypeOfPostFunc}
                t={t}
            />
        );

        const createPostButton = getByTitle('NewPost');
        fireEvent.click(createPostButton);

        const saveButton = getByTitle('Save');
        expect(saveButton).toBeInTheDocument();

        fireEvent.click(createPostButton);

        const cancelButton = getByTitle('Cancel');
        expect(cancelButton).toBeInTheDocument();
    });

    test('creates a new post when save button is clicked', async () => {
        const [createNewPostAsync] = useCreatePostAsyncMutation();

        const { getByTitle, } = render(
            <CreatePost
                user={user}
                owner={owner}
                postTypeName={postTypeName}
                createTypeOfPostFunc={createTypeOfPostFunc}
                t={t}
            />
        );

        const createPostButton = getByTitle('NewPost');
        fireEvent.click(createPostButton);

        const postContentInput = getByTitle('PostContent');
        fireEvent.change(postContentInput, { target: { value: 'Test post content' } });

        const saveButton = getByTitle('Save');
        fireEvent.click(saveButton);

        await waitFor(() => {
            expect(createNewPostAsync).toHaveBeenCalledWith({
                owner: owner,
                content: 'Test post content',
                postType: 0,
                tags: '',
                when: expect.any(Date),
                likeCount: 0,
                dislikeCount: 0,
                postComment: 0,
                appUserId: user.id,
            });
            expect(createTypeOfPostFunc).toHaveBeenCalledWith('789');
        });
    });

    test('does not create a new post when save button is clicked and createdPost.data is undefined', async () => {
        useCreatePostAsyncMutation.mockReturnValue([
            jest.fn().mockResolvedValue({ data: undefined }),
        ]);

        const [createNewPostAsync] = useCreatePostAsyncMutation();

        const { getByTitle } = render(
            <CreatePost
                user={user}
                owner={owner}
                postTypeName={postTypeName}
                createTypeOfPostFunc={createTypeOfPostFunc}
                t={t}
            />
        );

        const createPostButton = getByTitle('NewPost');
        fireEvent.click(createPostButton);

        const postContentInput = getByTitle('PostContent');
        fireEvent.change(postContentInput, { target: { value: 'Test post content' } });

        const saveButton = getByTitle('Save');
        fireEvent.click(saveButton);

        await waitFor(() => {
            expect(createNewPostAsync).toHaveBeenCalledWith({
                owner: owner,
                content: 'Test post content',
                postType: 0,
                tags: '',
                when: expect.any(Date),
                likeCount: 0,
                dislikeCount: 0,
                postComment: 0,
                appUserId: user.id,
            });
            expect(createTypeOfPostFunc).not.toHaveBeenCalledWith();
        });
    });
});
