import '@testing-library/jest-dom';
import { fireEvent, render, waitFor } from '@testing-library/react';
import { useCreateUserPostMutation } from '../../../store/api/post/UserPost.api';
import CreateUserPost from './CreateUserPost';

jest.mock('../../../store/api/post/UserPost.api');

describe('CreateUserPost', () => {
    const user: any = { id: '123' };
    const owner: string = 'owner';
    const t = (key: string) => key;

    beforeEach(() => {
        useCreateUserPostMutation.mockReturnValue([jest.fn()]);
    });

    test('renders create post button', () => {
        const { getByTitle } = render(
            <CreateUserPost user={user} owner={owner} t={t} />
        );

        const createPostButton = getByTitle('NewPost');
        expect(createPostButton).toBeInTheDocument();
    });

    test('toggles create post form when create post button is clicked', () => {
        const { getByTitle } = render(
            <CreateUserPost user={user} owner={owner} t={t} />
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
        const [createNewUserPostAsync] = useCreateUserPostMutation();

        const { getByTitle } = render(
            <CreateUserPost user={user} owner={owner} t={t} />
        );

        const createPostButton = getByTitle('NewPost');
        fireEvent.click(createPostButton);

        const postContentInput = getByTitle('PostContent');
        fireEvent.change(postContentInput, { target: { value: 'Test post content' } });

        const saveButton = getByTitle('Save');
        fireEvent.click(saveButton);

        await waitFor(() => {
            expect(createNewUserPostAsync).toHaveBeenCalledWith({
                owner: owner,
                content: 'Test post content',
                publicType: 0,
                tags: '',
                createdAt: expect.any(Date),
                likeCount: 0,
                dislikeCount: 0,
                postComment: 0,
                appUserId: user.id,
            });
        });
    });

    test('does not create a new post when save button is clicked and response data is undefined', async () => {
        useCreateUserPostMutation.mockReturnValue([
            jest.fn().mockResolvedValue({ data: undefined }),
        ]);

        const [createNewUserPostAsync] = useCreateUserPostMutation();

        const { getByTitle } = render(
            <CreateUserPost user={user} owner={owner} t={t} />
        );

        const createPostButton = getByTitle('NewPost');
        fireEvent.click(createPostButton);

        const postContentInput = getByTitle('PostContent');
        fireEvent.change(postContentInput, { target: { value: 'Test post content' } });

        const saveButton = getByTitle('Save');
        fireEvent.click(saveButton);

        await waitFor(() => {
            expect(createNewUserPostAsync).toHaveBeenCalledWith({
                owner: owner,
                content: 'Test post content',
                publicType: 0,
                tags: '',
                createdAt: expect.any(Date),
                likeCount: 0,
                dislikeCount: 0,
                postComment: 0,
                appUserId: user.id,
            });
        });
    });
});