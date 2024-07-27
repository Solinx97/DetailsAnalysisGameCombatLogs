import '@testing-library/jest-dom';
import { fireEvent, render } from '@testing-library/react';
import React from 'react';
import { useTranslation } from 'react-i18next';
import AddTagsToPost from './AddTagsToPost';

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

describe('AddTagsToPost Component', () => {
    const { t } = useTranslation();

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('renders tags correctly', () => {
        const postTags = ['tag1', 'tag2', 'tag3'];
        const setPostTags = jest.fn();

        const { getByText } = render(
            <AddTagsToPost
                postTags={postTags}
                setPostTags={setPostTags}
                t={t}
            />
        );

        postTags.forEach((tag) => {
            const tagElement = getByText(tag);
            expect(tagElement).toBeInTheDocument();
        });
    });

    test('adds tag when input is not empty and tags limit is not reached', () => {
        const postTags = ['tag1', 'tag2'];
        const setPostTags = jest.fn();


        const { getByPlaceholderText, container } = render(
            <AddTagsToPost
                postTags={postTags}
                setPostTags={setPostTags}
                t={t}
            />
        );

        const title = container.querySelector('.title');
        fireEvent.click(title);

        const tagInput = getByPlaceholderText('Enter tag');
        fireEvent.change(tagInput, { target: { value: 'newTag' } });

        fireEvent.blur(tagInput);

        expect(setPostTags).toHaveBeenCalledWith([...postTags, 'newTag']);
    });

    test('does not add tag when input is empty', () => {
        const postTags = ['tag1', 'tag2'];
        const setPostTags = jest.fn();

        const { getByPlaceholderText, container } = render(
            <AddTagsToPost
                postTags={postTags}
                setPostTags={setPostTags}
                t={t}
            />
        );

        const title = container.querySelector('.title');
        fireEvent.click(title);

        const tagInput = getByPlaceholderText('Enter tag');
        fireEvent.change(tagInput, { target: { value: '' } });

        fireEvent.blur(tagInput);

        expect(setPostTags).not.toHaveBeenCalled();
    });

    test('does not add tag when tags limit is reached', () => {
        const postTags = ['tag1', 'tag2', 'tag3', 'tag4', 'tag5'];
        const setPostTags = jest.fn();

        const { getByPlaceholderText, container } = render(
            <AddTagsToPost
                postTags={postTags}
                setPostTags={setPostTags}
                t={t}
            />
        );

        const title = container.querySelector('.title');
        fireEvent.click(title);

        const tagInput = getByPlaceholderText('Enter tag');
        fireEvent.change(tagInput, { target: { value: 'newTag' } });

        fireEvent.blur(tagInput);

        expect(setPostTags).not.toHaveBeenCalled();
    });

    test('removes tag when remove button is clicked', () => {
        const postTags = ['tag1', 'tag2', 'tag3'];
        const setPostTags = jest.fn();
        const { t } = useTranslation();

        const { getAllByTitle } = render(
            <AddTagsToPost
                postTags={postTags}
                setPostTags={setPostTags}
                t={t}
            />
        );

        const removeButtons = getAllByTitle('RemoveTag');
        expect(removeButtons.length).toBeGreaterThan(1);
        fireEvent.click(removeButtons[1]);

        expect(setPostTags).toHaveBeenCalledWith(['tag1', 'tag3']);
    });
});
