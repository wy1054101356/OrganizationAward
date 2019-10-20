/*
Navicat MySQL Data Transfer

Source Server         : 118.24.146.211
Source Server Version : 50640
Source Host           : 118.24.146.211:3306
Source Database       : organization

Target Server Type    : MYSQL
Target Server Version : 50640
File Encoding         : 65001

Date: 2018-07-27 20:40:18
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for tbl_organization
-- ----------------------------
DROP TABLE IF EXISTS `tbl_organization`;
CREATE TABLE `tbl_organization` (
  `oid` int(8) NOT NULL AUTO_INCREMENT COMMENT '组织id',
  `oname` varchar(100) CHARACTER SET utf8 NOT NULL COMMENT '组织名称',
  `created` int(16) NOT NULL,
  `status` int(4) NOT NULL DEFAULT '1' COMMENT '状态值：1正常，2删除',
  PRIMARY KEY (`oid`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of tbl_organization
-- ----------------------------
INSERT INTO `tbl_organization` VALUES ('1', '理想乡', '1532575678', '1');
INSERT INTO `tbl_organization` VALUES ('2', '吃枣药丸的云顶22', '1532600691', '2');
INSERT INTO `tbl_organization` VALUES ('3', '吃枣药丸的云顶11', '1532601125', '1');

-- ----------------------------
-- Table structure for tbl_record
-- ----------------------------
DROP TABLE IF EXISTS `tbl_record`;
CREATE TABLE `tbl_record` (
  `rid` int(8) NOT NULL AUTO_INCREMENT,
  `uid` int(8) NOT NULL,
  `uname` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `oid` int(8) NOT NULL,
  `oname` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `created` int(20) NOT NULL,
  `level` int(8) NOT NULL,
  `powernum` int(8) NOT NULL,
  `tongling` int(8) NOT NULL,
  `fighttime` int(8) NOT NULL,
  `lastweekdeduction` int(8) NOT NULL,
  `otherbonuspoints` int(8) NOT NULL,
  `otherdeduction` int(8) NOT NULL,
  `sumpoints` int(8) NOT NULL,
  `bagtype` int(8) NOT NULL,
  PRIMARY KEY (`rid`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of tbl_record
-- ----------------------------
INSERT INTO `tbl_record` VALUES ('3', '4', '红叶', '1', '理想乡', '1532686062', '105', '210000', '600', '4', '0', '100', '0', '3900', '0');
INSERT INTO `tbl_record` VALUES ('4', '4', '红叶', '1', '理想乡', '1532068185', '105', '210000', '600', '4', '0', '100', '0', '4700', '1');

-- ----------------------------
-- Table structure for tbl_reward
-- ----------------------------
DROP TABLE IF EXISTS `tbl_reward`;
CREATE TABLE `tbl_reward` (
  `id` int(8) NOT NULL AUTO_INCREMENT,
  `type` int(8) NOT NULL,
  `uid` int(8) NOT NULL,
  `uname` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `remark` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `points` int(20) NOT NULL,
  `created` int(8) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of tbl_reward
-- ----------------------------
INSERT INTO `tbl_reward` VALUES ('2', '1', '4', '红叶', '国士', '100', '1532685124');

-- ----------------------------
-- Table structure for tbl_user
-- ----------------------------
DROP TABLE IF EXISTS `tbl_user`;
CREATE TABLE `tbl_user` (
  `uid` int(8) NOT NULL AUTO_INCREMENT,
  `oid` int(8) NOT NULL,
  `oname` varchar(100) CHARACTER SET utf8 DEFAULT NULL,
  `uname` varchar(100) CHARACTER SET utf8 NOT NULL,
  `QQnumber` int(20) NOT NULL,
  `password` varchar(100) CHARACTER SET utf8 NOT NULL,
  `created` int(16) DEFAULT NULL,
  `status` int(4) NOT NULL DEFAULT '1',
  `user_type` int(4) NOT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of tbl_user
-- ----------------------------
INSERT INTO `tbl_user` VALUES ('1', '1', '理想乡', '希望', '1244202198', 'E10ADC3949BA59ABBE56E057F20F883E', '1532575678', '1', '1');
INSERT INTO `tbl_user` VALUES ('2', '1', '理想乡', '理想乡的希望', '825163048', 'E10ADC3949BA59ABBE56E057F20F883E', '1532575678', '1', '2');
INSERT INTO `tbl_user` VALUES ('3', '1', '理想乡', '爱理', '123456789', 'E10ADC3949BA59ABBE56E057F20F883E', '1532659242', '1', '3');
INSERT INTO `tbl_user` VALUES ('4', '1', '理想乡', '红叶', '1234567891', 'E10ADC3949BA59ABBE56E057F20F883E', '1532668274', '1', '3');
INSERT INTO `tbl_user` VALUES ('5', '1', '理想乡', '反倒是', '4545', 'E10ADC3949BA59ABBE56E057F20F883E', '1532688654', '1', '3');
