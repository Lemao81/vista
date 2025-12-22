'use client';

import React, { createContext, Dispatch, SetStateAction, useState } from 'react';
import SignUpModal from '@/components/auth/sign-up-modal';

export type ModalContextType = {
  setShowSignUpModal: Dispatch<SetStateAction<boolean>>;
};

export const ModalContext = createContext<ModalContextType>({
  setShowSignUpModal: () => {},
});

export default function ModalProvider({ children }: Readonly<{ children: React.ReactNode }>) {
  const [showSignUpModal, setShowSignUpModal] = useState(false);

  return (
    <ModalContext value={{ setShowSignUpModal }}>
      <SignUpModal show={showSignUpModal} />
      {children}
    </ModalContext>
  );
}
