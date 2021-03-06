﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sara;

namespace UnitTests
{
    [TestClass]
    public class TestBooleanExpr
    {
        [TestMethod]
        public void TestConstant()
        {
            Assert.AreEqual("true", Constant.True.ToString());
            Assert.AreEqual("false", Constant.False.ToString());
            Constant.True.Jumping(42, 99);
            Constant.False.Jumping(100, 8);
            //output :
            //goto L42
            //goto L8

            var c1 = new Constant(42);
            Assert.AreEqual("42", c1.ToString());
            var c2 = new Constant(new Real(3.14f), Sara.Type.Int);
            Assert.AreEqual("3.14", c2.ToString());
        }

        [TestMethod]
        public void TestLogical()
        {
            var logical = new Logical(Word.and, Constant.True, Constant.False);
            Assert.AreEqual("true && false", logical.ToString());
            logical.Gen();

            //output:	
            //iffalse true && false goto L1
            //        t1 = true
            //        goto L2
            //L1:	  t1 = false
            //L2:
        }

        [TestMethod]
        public void TestOr()
        {
            var or = new Or(Constant.False, Constant.False);
            Assert.AreEqual("false || false", or.ToString());

            or.Jumping(42, 99);
            //output:
            //  	  goto L99
        }

        [TestMethod]
        public void TestAnd()
        {
            var and = new And(Constant.True, Constant.True);
            Assert.AreEqual("true && true", and.ToString());

            and.Jumping(42, 99);
            //output:
            //  	  goto L42
        }

        [TestMethod]
        public void TestNot()
        {
            var not = new Not(Constant.True);
            Assert.AreEqual("! true", not.ToString());

            not.Jumping(42, 99);
            //output:
            //  	  goto L99
        }

        [TestMethod]
        public void TestRel()
        {
            var isLess = new Rel(new Token('<'), new Constant(10), new Constant(100));
            Assert.AreEqual("10 < 100", isLess.ToString());

            isLess.Jumping(42, 99);
            //output:
            //        if 10 < 100 goto L42
            //        goto L99
        }

        [TestMethod]
        public void TestAccess()
        {
            var acc = new Access(new Id(new Word("val", Tag.ID), Sara.Type.Int, 0xcf), new Constant(42), Sara.Type.Int);
            Assert.AreEqual("val [ 42 ]", acc.ToString());
            Assert.IsTrue(acc.Gen() is Access);
            acc.Jumping(10, 100);
            //output:
            //          t1 = val [ 42 ]
            //          if t1 goto L10
            //          goto L100
        }
    }
}
