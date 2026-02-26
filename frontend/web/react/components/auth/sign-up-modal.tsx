import { useContext } from 'react';
import SignUpForm from '@/components/auth/sign-up-form';
import Modal from '@/components/shared/modal';
import { ModalContext } from '@/components/shared/modal-provider';

export default function SignUpModal({ show }: { show: boolean }) {
  const { setShowSignUpModal } = useContext(ModalContext);

  return (
    <Modal onDismiss={() => setShowSignUpModal(false)} show={show}>
      <SignUpForm />
    </Modal>
  );
}
